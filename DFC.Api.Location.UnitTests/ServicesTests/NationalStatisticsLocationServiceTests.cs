using DFC.Api.Location.Models.NationalStatisticsLocationApiResponses;
using DFC.Api.Location.Services;
using DFC.Api.Location.UnitTests.FakeHttpHandler;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Api.Location.UnitTests.ServicesTests
{
    [Trait("Category", "National statistics location service tests")]
    public class NationalStatisticsLocationServiceTests
    {
        private readonly ILogger<NationalStatisticsLocationService> fakeLogger = A.Fake<ILogger<NationalStatisticsLocationService>>();

        [Fact]
        public async Task GetLocationsReturnsLocations()
        {
            //Setup
            var expectedResponse = GetTestResponse();

            var httpResponse = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse)),
                StatusCode = HttpStatusCode.Accepted,
            };

            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler);
            httpClient.BaseAddress = new System.Uri("https://dummy.com");

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            var nationalStatisticsLocationService = new NationalStatisticsLocationService(fakeLogger, httpClient);

            //Act
            var actual = await nationalStatisticsLocationService.GetLocationsAsync().ConfigureAwait(false);

            //Assert
            actual.Should().BeEquivalentTo(expectedResponse.Locations);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        private OnsLocationResponse GetTestResponse()
        {
            return new OnsLocationResponse()
            {
                ExceededTransferLimit = false,
                Locations = new List<LocationsResponse>()
                {
                    new LocationsResponse()
                    {
                        Location = new LocationResponse
                        {
                            Id = 1,
                            LocationName = "LN1",
                            LocalAuthorityName = "LAN1",
                            LocationAuthorityDistrict = "LAD1",
                            Longitude = 1,
                            Latitude = 2,
                        },
                    },
                },
            };
        }
    }
}
