using DFC.Api.Location.Contracts;
using DFC.Api.Location.Functions;
using DFC.Api.Location.Models.NationalStatisticsLocationApiResponses;
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
    [Trait("Category", "Post - Http location data load tests")]
    public class LocationDataLoadTests
    {
        private readonly ILogger<LocationDataLoad> fakeLogger = A.Fake<ILogger<LocationDataLoad>>();
        private readonly ILoadLocations fakeLoadLocations = A.Fake<ILoadLocations>();

        [Fact]
        public async Task LocationDataLoadReturnsNumberLoaded()
        {
            //Setup
            var expectedNumber = 123;
            A.CallTo(() => fakeLoadLocations.GetLocationsAndUpdateIndex()).Returns(expectedNumber);
            var function = new LocationDataLoad(fakeLogger, fakeLoadLocations);

            //Act
            var result = await function.Run(new DefaultHttpRequest(new DefaultHttpContext())).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeLoadLocations.GetLocationsAndUpdateIndex()).MustHaveHappenedOnceExactly();

            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okResult.Value.ToString().Should().Contain($"Loaded {expectedNumber} Locations");
        }

        private IEnumerable<LocationResponse?> GetTestCleanLocations()
        {
            yield return new LocationResponse()
            {
                Id = 123,
                LocationName = "LN1",
                LocalAuthorityName = "LAN1",
                LocationAuthorityDistrict = "LAD1",
            };
        }
    }
}
