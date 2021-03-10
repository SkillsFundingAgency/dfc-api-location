using AzureFunctions.Extensions.Swashbuckle;
using DFC.Api.Location;
using DFC.Api.Location.Contracts;
using DFC.Api.Location.Extensions;
using DFC.Api.Location.HttpClientPolicies;
using DFC.Api.Location.Models.HttpClientConfig;
using DFC.Api.Location.Services;
using DFC.Swagger.Standard;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[assembly: WebJobsStartup(typeof(WebJobsExtensionStartup), "Web Jobs Extension Startup")]

namespace DFC.Api.Location
{
    [ExcludeFromCodeCoverage]
    public class WebJobsExtensionStartup : IWebJobsStartup
    {
        private const string AppSettingsPolicies = "Policies";

        public void Configure(IWebJobsBuilder builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddHttpClient();

            var policyOptions = configuration.GetSection(AppSettingsPolicies).Get<PolicyOptions>() ?? new PolicyOptions();
            var policyRegistry = builder.Services.AddPolicyRegistry();

            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
            builder.Services.AddApplicationInsightsTelemetry();
            builder.Services.AddTransient<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();
            builder.Services.AddTransient<INationalStatisticsLocationService, NationalStatisticsLocationService>();
            builder.Services.AddTransient<ILoadLocationsService, LoadLocationsService>();

            builder.Services
                .AddPolicies(policyRegistry, nameof(OnsHttpClientOptions), policyOptions)
                .AddHttpClient<INationalStatisticsLocationService, NationalStatisticsLocationService, OnsHttpClientOptions>(configuration, nameof(OnsHttpClientOptions), nameof(PolicyOptions.HttpRetry), nameof(PolicyOptions.HttpCircuitBreaker));
        }
    }
}
