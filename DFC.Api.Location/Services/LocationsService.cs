using DFC.Api.Location.Contracts;
using DFC.Api.Location.Models.NationalStatisticsLocationApiResponses;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Api.Location.Services
{
    public class LocationsService : ILocationsService
    {
        private readonly ILogger<LocationsService> logger;

        private readonly INationalStatisticsLocationService nationalStatisticsLocationService;

        public LocationsService(ILogger<LocationsService> logger, INationalStatisticsLocationService nationalStatisticsLocationService)
        {
            this.logger = logger;
            this.nationalStatisticsLocationService = nationalStatisticsLocationService;
        }

        public async Task<IEnumerable<LocationResponse?>> GetCleanLocationsAsync()
        {
            logger.LogInformation("Getting data from ONS");

            var locations = await nationalStatisticsLocationService.GetLocationsAsync().ConfigureAwait(false);

            logger.LogInformation($"Got data from ONS {locations.Count()} records");

            var cleanedItems = locations.Where(item => !string.IsNullOrEmpty(item.Location?.LocationName))
                    .Where(item => !string.IsNullOrEmpty(item.Location?.LocalAuthorityName))
                    .Where(item => !string.IsNullOrEmpty(item.Location?.LocationAuthorityDistrict))
                    .GroupBy(c => new { c.Location?.LocalAuthorityName, c.Location?.LocationName, c.Location?.LocationAuthorityDistrict })
                    .Select(item => item.First())
                    .GroupBy(c => new { c.Location?.Id })
                    .Select(item => item.FirstOrDefault())
                    .Select(item => item.Location);

            logger.LogInformation($"After data cleaning there are {locations.Count()} records");

            return cleanedItems;
        }
    }
}
