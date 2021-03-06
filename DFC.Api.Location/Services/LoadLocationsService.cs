using DFC.Api.Location.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DFC.Api.Location.Services
{
    public class LoadLocationsService : ILoadLocationsService
    {
        private readonly ILogger<LoadLocationsService> logger;

        private readonly INationalStatisticsLocationService nationalStatisticsLocationService;

        public LoadLocationsService(ILogger<LoadLocationsService> logger, INationalStatisticsLocationService nationalStatisticsLocationService)
        {
            this.logger = logger;
            this.nationalStatisticsLocationService = nationalStatisticsLocationService;
        }

        public async Task<int> LoadLocations()
        {
            logger.LogInformation("Getting data from ONS");

            var locations = await nationalStatisticsLocationService.GetLocations().ConfigureAwait(false);

            logger.LogInformation($"Got data from ONS {locations.Count()} records");

            var cleanedItems = locations.Where(item => !string.IsNullOrEmpty(item.Location?.LocationName))
                    .Where(item => !string.IsNullOrEmpty(item.Location?.LocalAuthorityName))
                    .Where(item => !string.IsNullOrEmpty(item.Location?.LocationAuthorityDistrict))
                    .GroupBy(c => new { c.Location?.LocalAuthorityName, c.Location?.LocationName, c.Location?.LocationAuthorityDistrict })
                    .Select(item => item.First())
                    .GroupBy(c => new { c.Location?.Id })
                    .Select(item => item.FirstOrDefault())
                    .ToList();

            logger.LogInformation($"After data cleaning there are {locations.Count()} records");
            return cleanedItems.Count;
        }
    }
}
