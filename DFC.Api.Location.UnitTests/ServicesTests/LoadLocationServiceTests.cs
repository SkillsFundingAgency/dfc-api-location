using DFC.Api.Location.Contracts;
using DFC.Api.Location.Models.NationalStatisticsLocationApiResponses;
using DFC.Api.Location.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.Location.UnitTests.ServicesTests
{
    [Trait("Category", "Load location service tests")]
    public class LoadLocationServiceTests
    {
        private readonly ILogger<LocationsService> fakeLogger = A.Fake<ILogger<LocationsService>>();
        private readonly INationalStatisticsLocationService fakeNationalStatisticsLocationService = A.Fake<INationalStatisticsLocationService>();

        [Theory]
        [InlineData(2, "LN2", "LAN2", "LAD2", 2)]
        [InlineData(1, "LN1", "LAN1", "LAD1", 1)]
        [InlineData(2, "LN2", "LAN2", "", 1)]
        [InlineData(2, "LN2", "", "LAD2", 1)]
        [InlineData(2, "", "LAN2", "LAD2", 1)]
        [InlineData(1, "LN2", "LAN2", "LAD2", 1)]
        public async Task LoadLocationsCleansData(int locationId, string locationName, string localAuthorityName, string locationAuthorityDistrict, int expectedNumberOfLocations)
        {
            //Setup
            A.CallTo(() => fakeNationalStatisticsLocationService.GetLocations()).Returns(GetTestLocations(locationId, locationName, localAuthorityName, locationAuthorityDistrict));
            var loadLocationsService = new LocationsService(fakeLogger, fakeNationalStatisticsLocationService);

            //Act
            var result = await loadLocationsService.GetCleanLocations().ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeNationalStatisticsLocationService.GetLocations()).MustHaveHappenedOnceExactly();
            result.Count().Should().Be(expectedNumberOfLocations);
        }

        private IEnumerable<LocationsResponse> GetTestLocations(int locationId, string locationName, string localAuthorityName, string locationAuthorityDistrict)
        {
            yield return new LocationsResponse()
            {
                Location = GetLocation(1, "LN1", "LAN1", "LAD1"),
            };

            yield return new LocationsResponse()
            {
                Location = GetLocation(locationId, locationName, localAuthorityName, locationAuthorityDistrict),
            };
        }

        private LocationResponse GetLocation(int locationId, string locationName, string localAuthorityName, string locationAuthorityDistrict)
        {
            return new LocationResponse()
            {
                Id = locationId,
                LocationName = locationName,
                LocalAuthorityName = localAuthorityName,
                LocationAuthorityDistrict = locationAuthorityDistrict,
            };
        }
    }
}
