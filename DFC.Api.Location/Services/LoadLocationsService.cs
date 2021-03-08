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
            var locations = await nationalStatisticsLocationService.GetLocations().ConfigureAwait(false);

            var cleanedItems = locations.ToList().Where(item => !string.IsNullOrEmpty(item.Location?.LocationName))
                    .Where(item => !string.IsNullOrEmpty(item.Location?.LocalAuthorityName))
                    .Where(item => !string.IsNullOrEmpty(item.Location?.LocationAuthorityDistrict))
                    .GroupBy(c => new { c.Location?.LocalAuthorityName, c.Location?.LocationName, c.Location?.LocationAuthorityDistrict })
                    .Select(item => item.First())
                    .GroupBy(c => new { c.Location?.Id })
                    .Select(item => item.FirstOrDefault())
                    .ToList();

            //var featureItems = locations.ToList().Select(a => a.Location);
            //WriteCSV(featureItems, @"C:\rawlocations.txt");
            //WriteCSV(cleanedItems.Select(a => a.Location), @"C:\cleanlocations.txt");
            return cleanedItems.Count;
        }

        //This is just for debugging will be removed
        private static void WriteCSV<T>(IEnumerable<T> items, string path)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .OrderBy(p => p.Name);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(string.Join("|", props.Select(p => p.Name)));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join("|", props.Select(p => p.GetValue(item, null))));
                }
            }
        }
    }
}
