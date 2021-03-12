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

        public SearchIndexService(ILogger<NationalStatisticsLocationService> logger, AzureSearchIndexConfig azureSearchIndexConfig)
        {
            this.logger = logger;
            this.azureSearchIndexConfig = azureSearchIndexConfig;
        }

        public async Task<int> BuildIndexAsync(IEnumerable<SearchLocationIndex> searchLocations)
        {
            logger.LogInformation($"Starting to build index for {searchLocations.Count()}");

            if (azureSearchIndexConfig.SearchServiceAdminAPIKey == null)
            {
                throw new DFCNullConfigValueException(nameof(azureSearchIndexConfig.SearchServiceAdminAPIKey));
            }

            try
            {
                AzureKeyCredential credential = new AzureKeyCredential(azureSearchIndexConfig.SearchServiceAdminAPIKey);
                var searchIndexClient = new SearchIndexClient(azureSearchIndexConfig.EndpointUri, credential);
                var searchClient = new SearchClient(azureSearchIndexConfig.EndpointUri, azureSearchIndexConfig.LocationSearchIndex, credential);
                var fieldBuilder = new FieldBuilder();
                var searchFields = fieldBuilder.Build(typeof(SearchLocationIndex));
                var definition = new SearchIndex(azureSearchIndexConfig.LocationSearchIndex, searchFields);
                var suggester = new SearchSuggester("sg", new[] { "LocationName", "LocationAuthorityDistrict" });
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
                    throw new DFCIndexUploadException("sampleMessage");
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
    }
}
