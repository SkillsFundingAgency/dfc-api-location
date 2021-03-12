using DFC.Api.Location.Contracts;
using DFC.Api.Location.Models.NationalStatisticsLocationApiResponses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Api.Location.Services
{
    public class NationalStatisticsLocationService : INationalStatisticsLocationService
    {
        private const string NationalOfficeOfStatisticsLocationQuery = "query?where=ctry15nm%20%3D%20'ENGLAND'%20AND%20popcnt%20%3E%3D%20500%20AND%20popcnt%20%3C%3D%2010000000&outFields=placeid,place15nm,ctry15nm,cty15nm,ctyltnm,lad15nm,lat,long,&returnDistinctValues=true&outSR=4326&f=json";
        private readonly ILogger<NationalStatisticsLocationService> logger;
        private readonly HttpClient httpClient;

        public NationalStatisticsLocationService(ILogger<NationalStatisticsLocationService> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<LocationsResponse>> GetLocationsAsync()
        {
            var moreData = true;
            var locations = new List<LocationsResponse>();
            var offSet = 0;
            var numbertoReturn = 2000;

            while (moreData)
            {
                var requestUri = new Uri($"{httpClient.BaseAddress}{NationalOfficeOfStatisticsLocationQuery}&resultRecordCount={numbertoReturn}&resultOffSet={offSet}");

                logger.LogInformation($"Making request to {requestUri}");

                var response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var locationsReturned = JsonConvert.DeserializeObject<OnsLocationResponse>(jsonResponse);

                logger.LogInformation($"ONS API returned {locationsReturned.Locations?.Count}");

                moreData = locationsReturned.ExceededTransferLimit;

                offSet += numbertoReturn;

                if (locationsReturned.Locations != null)
                {
                    locations.AddRange(locationsReturned.Locations);
                }
            }

            logger.LogInformation($"Completed getting locations from API");

            return locations;
        }
    }
}
