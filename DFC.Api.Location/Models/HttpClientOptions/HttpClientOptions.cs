using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.Api.Location.Models.HttpClientOptions
{
    public abstract class HttpClientOptions
    {
        public Uri? BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 20);         // default to 20 seconds
    }
}
