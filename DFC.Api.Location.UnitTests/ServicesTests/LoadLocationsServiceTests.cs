using AutoMapper;
using DFC.Api.Location.Contracts;
using DFC.Api.Location.Models.AzureSearch;
using DFC.Api.Location.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.Location.UnitTests.ServicesTests
{
    [Trait("Category", "Load location service tests")]
    public class LoadLocationsServiceTests
    {
        private readonly ILogger<LoadLocationsService> fakeLogger = A.Fake<ILogger<LoadLocationsService>>();

        private readonly ILocationsService fakeLocationsService = A.Fake<ILocationsService>();

        private readonly ISearchIndexService fakeSearchIndexService = A.Fake<ISearchIndexService>();

        private readonly IMapper fakeMapper = A.Fake<IMapper>();

        [Fact]
        public async void GetLocationsAndUpdateIndexTest()
        {
            //Setup
            var expectedNumberOfLocations = 123;
            A.CallTo(() => fakeSearchIndexService.BuildIndexAsync(A<IEnumerable<SearchLocationIndex>>.Ignored)).Returns(expectedNumberOfLocations);
            var loadLocationsService = new LoadLocationsService(fakeLogger, fakeLocationsService, fakeSearchIndexService, fakeMapper);

            //Act
            var result = await loadLocationsService.GetLocationsAndUpdateIndexAsync().ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeLocationsService.GetCleanLocationsAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeSearchIndexService.BuildIndexAsync(A<IEnumerable<SearchLocationIndex>>.Ignored)).MustHaveHappenedOnceExactly();
            result.Should().Be(expectedNumberOfLocations);
        }

        [Fact]
        public void GetLocationsAndUpdateIndexThrowsExceptionOnErrorTest()
        {
            //Setup
            A.CallTo(() => fakeSearchIndexService.BuildIndexAsync(A<IEnumerable<SearchLocationIndex>>.Ignored)).Throws(new ApplicationException());
            var loadLocationsService = new LoadLocationsService(fakeLogger, fakeLocationsService, fakeSearchIndexService, fakeMapper);

            //Act
            Func<Task> action = async () => await loadLocationsService.GetLocationsAndUpdateIndexAsync().ConfigureAwait(false);

            //Assert
            action.Should().Throw<ApplicationException>();
        }
    }
}
