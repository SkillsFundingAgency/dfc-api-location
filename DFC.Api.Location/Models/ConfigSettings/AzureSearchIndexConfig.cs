using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Api.Location.Models.ConfigSettings
{
    [ExcludeFromCodeCoverage]
    public class AzureSearchIndexConfig
    {
        public Uri? EndpointUri { get; set; }

        public string? LocationSearchIndex { get; set; }

        public string? SearchServiceName { get; set; }

        public string? SearchServiceAdminAPIKey { get; set; }
    }
}
