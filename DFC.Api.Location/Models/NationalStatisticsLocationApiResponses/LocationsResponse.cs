using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.Api.Location.Models.NationalStatisticsLocationApiResponses
{
    public class LocationsResponse
    {
        [JsonProperty("attributes")]
        public LocationResponse? Location { get; set; }
    }
}
