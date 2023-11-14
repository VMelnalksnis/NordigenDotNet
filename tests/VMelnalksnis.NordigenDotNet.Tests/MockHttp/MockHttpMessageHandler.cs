using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Tests.MockHttp;

public class MockHttpMessageHandler : HttpMessageHandler
{
	private readonly string _json;

	public MockHttpMessageHandler(string json)
	{
		_json = json;
	}

	protected override Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		return Task.FromResult(new HttpResponseMessage
		{
			Content = new StringContent(_json),
			StatusCode = HttpStatusCode.OK,
		});
	}
}
