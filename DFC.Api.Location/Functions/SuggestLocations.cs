using AutoMapper;
using DFC.Api.Location.Contracts;
using DFC.Api.Location.Models.APIModels;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Api.Location.Functions
{
        public class SuggestLocations
        {
            private readonly ILogger<SuggestLocations> logger;
            private readonly ISearchIndexService searchIndexService;
            private readonly IMapper mapper;

            public SuggestLocations(ILogger<SuggestLocations> logger, ISearchIndexService searchIndexService, IMapper mapper)
            {
                this.logger = logger;
                this.searchIndexService = searchIndexService;
                this.mapper = mapper;
            }

            [FunctionName("SuggestLocations")]
            [Display(Name = "Suggest Locations", Description = "Suggest matching location for a given term")]
            [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Suggestions returned", ShowSchema = false)]
            public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "suggestlocations/{term}")] HttpRequest req, string term)
            {
                logger.LogInformation($"Starting suggest locations with term {term} {req?.Body}");

                var locationResults = await searchIndexService.SuggestAsync(term).ConfigureAwait(false);
                var suggestedLocations = mapper.Map<IEnumerable<SuggestedLocations>>(locationResults);

                logger.LogInformation("Completed suggest locations");

                return new OkObjectResult(suggestedLocations);
            }
        }
}
