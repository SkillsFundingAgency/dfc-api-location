using DFC.Api.Location.Contracts;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.Location.Functions
{
    public class LocationDataLoad
    {
        private readonly ILogger<LocationDataLoad> logger;
        private readonly ILoadLocations loadLocations;

        public LocationDataLoad(ILogger<LocationDataLoad> logger, ILoadLocations loadLocations)
        {
            this.logger = logger;
            this.loadLocations = loadLocations;
        }

        [FunctionName("LoadLocations")]
        [Display(Name = "Load location data ", Description = "Get location data from ONS and load in to azure index.")]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Location data loaded", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is unknown or invalid", ShowSchema = false)]
        public async Task<IActionResult>
            Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            logger.LogInformation($"Starting loaded locations with {req?.Body}");

            var numberLoaded = await loadLocations.GetLocationsAndUpdateIndex().ConfigureAwait(false);

            logger.LogInformation("Completed loaded locations");

            return new OkObjectResult($"Loaded {numberLoaded} Locations");
        }
    }
}