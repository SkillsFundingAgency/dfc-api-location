using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.Api.Location.Models.ConfigSettings
{
    public class AzureSearchIndexConfig
    {
        public Uri? EndpointUri { get; set; }

        public string? LocationSearchIndex { get; set; }

        public string? SearchServiceName { get; set; }

        public string? SearchServiceAdminAPIKey { get; set; }
    }
}
