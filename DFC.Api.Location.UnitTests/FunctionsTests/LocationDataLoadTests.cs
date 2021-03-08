using DFC.Api.Location.Contracts;
using DFC.Api.Location.Functions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.Location.UnitTests.FunctionsTests
{
    [Trait("Category", "Post - Http location data load tests")]
    public class LocationDataLoadTests
    {
        private readonly ILogger<LocationDataLoad> fakeLogger = A.Fake<ILogger<LocationDataLoad>>();
        private readonly ILoadLocationsService fakeLoadLocationsService = A.Fake<ILoadLocationsService>();

        [Fact]
        public async Task LocationDataLoadReturnsNumberLoaded()
        {
            //Setup
            A.CallTo(() => fakeLoadLocationsService.LoadLocations()).Returns(123);
            var function = new LocationDataLoad(fakeLogger, fakeLoadLocationsService);

            //Act
            var result = await function.Run(new DefaultHttpRequest(new DefaultHttpContext())).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeLoadLocationsService.LoadLocations()).MustHaveHappenedOnceExactly();

            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okResult.Value.ToString().Should().Contain("123");
        }
    }
}
