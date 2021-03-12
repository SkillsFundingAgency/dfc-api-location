using AutoMapper;
using DFC.Api.Location.Contracts;
using DFC.Api.Location.Models.AzureSearch;
using DFC.Api.Location.Models.NationalStatisticsLocationApiResponses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Api.Location.Services
{
    public class LoadLocationsService : ILoadLocations
    {
        private readonly ILogger<LoadLocationsService> logger;

        private readonly ILocationsService locationsService;

        private readonly ISearchIndexService searchIndexService;

        private readonly IMapper mapper;

        public LoadLocationsService(ILogger<LoadLocationsService> logger, ILocationsService locationsService, ISearchIndexService searchIndexService, IMapper mapper)
        {
            this.logger = logger;
            this.locationsService = locationsService;
            this.searchIndexService = searchIndexService;
            this.mapper = mapper;
        }

        public async Task<int> GetLocationsAndUpdateIndexAsync()
        {
            logger.LogInformation("Starting to get locations and update index");
            try
            {
                var cleanLocations = await locationsService.GetCleanLocationsAsync().ConfigureAwait(false);
                logger.LogInformation($"Got {cleanLocations.Count()} locations");
                var searchLocations = mapper.Map<IEnumerable<SearchLocationIndex>>(cleanLocations);
                return await searchIndexService.BuildIndexAsync(searchLocations).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError("GetLocationsAndUpdateIndex", ex);
                throw;
            }
        }
    }
}
