using System.Net.Http;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Serialization;

internal static class HttpResponseMessageExtensions
{
	internal static async Task ThrowIfNotSuccessful(this HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			return;
		}

		var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#if NET6_0_OR_GREATER
		throw new HttpRequestException(content, null, response.StatusCode);
#else
		throw new HttpRequestException(content);
#endif
	}
}
