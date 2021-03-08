using Newtonsoft.Json;

namespace DFC.Api.Location.Models.NationalStatisticsLocationApiResponses
{
    public class LocationsResponse
    {
        [JsonProperty("attributes")]
        public LocationResponse? Location { get; set; }
    }
}
