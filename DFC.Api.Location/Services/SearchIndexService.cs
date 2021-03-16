using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using DFC.Api.Location.Contracts;
using DFC.Api.Location.CutomExceptions;
using DFC.Api.Location.Models.AzureSearch;
using DFC.Api.Location.Models.ConfigSettings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Api.Location.Services
{
    [ExcludeFromCodeCoverage]
    public class SearchIndexService : ISearchIndexService
    {
        private readonly ILogger<NationalStatisticsLocationService> logger;
        private readonly AzureSearchIndexConfig azureSearchIndexConfig;
        private readonly string suggestorName = "sglocation";

        public SearchIndexService(ILogger<NationalStatisticsLocationService> logger, AzureSearchIndexConfig azureSearchIndexConfig)
        {
            this.logger = logger;
            this.azureSearchIndexConfig = azureSearchIndexConfig;
        }

        public async Task<int> BuildIndexAsync(IEnumerable<SearchLocationIndex> searchLocations)
        {
            logger.LogInformation($"Starting to build index for {searchLocations.Count()}");
            try
            {
                var searchIndexClient = new SearchIndexClient(azureSearchIndexConfig.EndpointUri, GetAzureKeyCredential());
                var searchClient = new SearchClient(azureSearchIndexConfig.EndpointUri, azureSearchIndexConfig.LocationSearchIndex, GetAzureKeyCredential());
                var fieldBuilder = new FieldBuilder();
                var searchFields = fieldBuilder.Build(typeof(SearchLocationIndex));
                var definition = new SearchIndex(azureSearchIndexConfig.LocationSearchIndex, searchFields);
                var suggester = new SearchSuggester(suggestorName, new[] { nameof(SearchLocationIndex.LocationName), nameof(SearchLocationIndex.LocationAuthorityDistrict) });
                definition.Suggesters.Add(suggester);

                logger.LogInformation("created search objects and creating index");
                await searchIndexClient.CreateOrUpdateIndexAsync(definition).ConfigureAwait(false);
                logger.LogInformation("Created search index and uploading documents");

                var batch = IndexDocumentsBatch.Upload(searchLocations);
                IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch).ConfigureAwait(false);

                var failedRecords = result.Results.Where(r => !r.Succeeded);
                if (failedRecords.Any())
                {
                    var sampleFailedRecord = failedRecords.FirstOrDefault();
                    var sampleMessage = $"{failedRecords.Count()} have failed to upload to the index, sample failed record  message {sampleFailedRecord.ErrorMessage}, Status = {sampleFailedRecord.Status}";
                    logger.LogError(sampleMessage);
                    throw new DfcIndexUploadException("sampleMessage");
                }

                logger.LogInformation($"Created search index and uploaded {result.Results.Count} documents");

                return result.Results.Count;
            }
            catch (Exception ex)
            {
                logger.LogError("Building index had an error", ex);
                throw;
            }
        }

        public async Task<IEnumerable<SearchLocationIndex>> SuggestAsync(string term)
        {

            logger.LogInformation($"Starting to get suggestions for {term}");
            try
            {
                var suggestOptions = new SuggestOptions()
                {
                    Size = 5,
                };

                suggestOptions.Select.Add(nameof(SearchLocationIndex.LocationId));
                suggestOptions.Select.Add(nameof(SearchLocationIndex.LocationName));
                suggestOptions.Select.Add(nameof(SearchLocationIndex.LocationAuthorityDistrict));
                suggestOptions.Select.Add(nameof(SearchLocationIndex.LocalAuthorityName));
                suggestOptions.Select.Add(nameof(SearchLocationIndex.Longitude));
                suggestOptions.Select.Add(nameof(SearchLocationIndex.Latitude));

                var searchClient = new SearchClient(azureSearchIndexConfig.EndpointUri, azureSearchIndexConfig.LocationSearchIndex, GetAzureKeyCredential());

                var suggestResults = await searchClient.SuggestAsync<SearchLocationIndex>(term, suggestorName, suggestOptions)
                  .ConfigureAwait(false);
                return suggestResults.Value.Results.Select(i => i.Document);
            }
            catch (Exception ex)
            {
                logger.LogError("Getting suggestions had an error", ex);
                throw;
            }
        }

        private AzureKeyCredential GetAzureKeyCredential()
        {
            if (azureSearchIndexConfig.SearchServiceAdminAPIKey == null)
            {
                throw new DfcNullConfigValueException(nameof(azureSearchIndexConfig.SearchServiceAdminAPIKey));
            }

            return new AzureKeyCredential(azureSearchIndexConfig.SearchServiceAdminAPIKey);
        }
    }
}
