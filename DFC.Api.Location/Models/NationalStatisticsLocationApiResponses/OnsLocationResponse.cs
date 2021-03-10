using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.Api.Location.Models.NationalStatisticsLocationApiResponses
{
    public class OnsLocationResponse
    {
        [JsonProperty("features")]
        public List<LocationsResponse>? Locations { get; set; }

        [JsonProperty("exceededTransferLimit")]
        public bool ExceededTransferLimit { get; set; }
    }
}