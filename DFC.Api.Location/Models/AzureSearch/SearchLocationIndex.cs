﻿using Azure.Search.Documents.Indexes;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.Location.Models.AzureSearch
{
    [ExcludeFromCodeCoverage]
    public partial class SearchLocationIndex
    {
        [SimpleField(IsKey = true, IsFilterable = true)]
        public string? LocationId { get; set; }

        [SearchableField(IsSortable = true, IsFilterable = true)]
        public string? LocationName { get; set; }

        [SearchableField(IsSortable = true, IsFilterable = true)]
        public string? LocalAuthorityName { get; set; }

        [SearchableField(IsSortable = true, IsFilterable = true)]
        public string? LocationAuthorityDistrict { get; set; }

        [SimpleField(IsFilterable = true)]
        public double Latitude { get; set; }

        [SimpleField(IsFilterable = true)]
        public double Longitude { get; set; }
    }
}
