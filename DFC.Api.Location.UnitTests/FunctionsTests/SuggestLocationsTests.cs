using AutoMapper;
using DFC.Api.Location.AutoMapperProfiles;
using DFC.Api.Location.Contracts;
using DFC.Api.Location.Functions;
using DFC.Api.Location.Models.APIModels;
using DFC.Api.Location.Models.AzureSearch;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.Location.UnitTests.FunctionsTests
{
    [Trait("Category", "Get - Http suggest locations tests")]
    public class SuggestLocationsTests
    {
        private readonly ILogger<SuggestLocations> fakeLogger = A.Fake<ILogger<SuggestLocations>>();
        private readonly ISearchIndexService fakeSearchIndexService = A.Fake<ISearchIndexService>();

        [Fact]
        public async Task LocationDataLoadReturnsNumberLoaded()
        {
            //Setup
            var testSuggestedLocations = GetTestSuggestedLocations();
            A.CallTo(() => fakeSearchIndexService.SuggestAsync(A<string>.Ignored)).Returns(testSuggestedLocations);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<LocationModelProfile>());
            var mapper = config.CreateMapper();
            var expectedSuggestions = mapper.Map<IEnumerable<SuggestedLocation>>(testSuggestedLocations);
            var function = new SuggestLocations(fakeLogger, fakeSearchIndexService, mapper);

            //Act
            var result = await function.Run(new DefaultHttpRequest(new DefaultHttpContext()), "testTerm").ConfigureAwait(false);

            //Asserts
            A.CallTo(() => fakeSearchIndexService.SuggestAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            okResult.Value.Should().BeEquivalentTo(expectedSuggestions);
        }

        private IEnumerable<SearchLocationIndex> GetTestSuggestedLocations()
        {
            yield return new SearchLocationIndex()
            {
                LocationId = "123",
                LocationName = "LN1",
                LocalAuthorityName = "LAN1",
                LocationAuthorityDistrict = "LAD1",
                Longitude = 1.23,
                Latitude = 4.56,
            };
        }
    }
}
