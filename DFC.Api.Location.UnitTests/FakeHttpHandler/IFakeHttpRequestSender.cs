using System.Net.Http;

namespace DFC.Api.Location.UnitTests.FakeHttpHandler
{
    public interface IFakeHttpRequestSender
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}
