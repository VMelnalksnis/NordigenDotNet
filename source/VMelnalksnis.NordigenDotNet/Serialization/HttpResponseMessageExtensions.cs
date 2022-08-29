// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

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
		throw new HttpRequestException(content, null, response.StatusCode);
	}
}
