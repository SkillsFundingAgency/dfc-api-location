using DFC.Swagger.Standard.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.Api.Location.Models.APIModels
{
    public class SuggestedLocations
    {
        [Example(Description = "Location Id")]
        public string? LocationId { get; set; }

        [Example(Description = "Location name")]
        public string? LocationName { get; set; }

        [Example(Description = "Local authority name")]
        public string? LocalAuthorityName { get; set; }

        [Example(Description = "Location authority district")]
        public string? LocationAuthorityDistrict { get; set; }

        [Example(Description = "Latitude of location")]
        public double Latitude { get; set; }

        [Example(Description = "Longitude of location")]
        public double Longitude { get; set; }
    }
}
