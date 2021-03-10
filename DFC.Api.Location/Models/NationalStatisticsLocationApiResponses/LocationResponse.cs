using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.Api.Location.Models.NationalStatisticsLocationApiResponses
{
    public class LocationResponse
    {
        [JsonProperty("placeid")]
        public int? Id { get; set; }

        [JsonProperty("place15nm")]
        public string? LocationName { get; set; }

        [JsonProperty("ctyltnm")]
        public string? LocalAuthorityName { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("long")]
        public double Longitude { get; set; }

        [JsonProperty("lad15nm")]
        public string? LocationAuthorityDistrict { get; set; }
    }
}
