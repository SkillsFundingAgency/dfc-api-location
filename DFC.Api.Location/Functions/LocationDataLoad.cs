using DFC.Api.Location.Contracts;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

public class LocationDataLoad
{
    private readonly ILogger<LocationDataLoad> logger;
    private readonly ILoadLocationsService loadLocationsService;

    public LocationDataLoad(ILogger<LocationDataLoad> logger, ILoadLocationsService loadLocationsService)
    {
        this.logger = logger;
        this.loadLocationsService = loadLocationsService;
    }

    [FunctionName("LoadLocations")]
    [Display(Name = "Load location data ", Description = "Get location data from ONS and load in to azure index.")]
    [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Location data loaded", ShowSchema = false)]
    [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is unknown or invalid", ShowSchema = false)]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        var numberLoaded = await loadLocationsService.LoadLocations().ConfigureAwait(false);

        return new OkObjectResult($"Loaded {numberLoaded} Locations");
    }
}