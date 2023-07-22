namespace ApiReportApi.Tests.Mocks;

public class MockHttpClient : HttpClient
{
    public MockHttpClient(HttpResponseMessage response) : base(new MockHttpMessageHandler(response))
    {
    }
}