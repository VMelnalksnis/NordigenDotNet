// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Tokens;

internal sealed class TokenClient
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

	internal TokenClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	internal async Task<Token> New(TokenCreation tokenCreation)
	{
		var tokenResponse = await _httpClient
			.PostAsJsonAsync(Routes.Tokens.New, tokenCreation, _serializerOptions)
			.ConfigureAwait(false);

		if (tokenResponse.StatusCode is not HttpStatusCode.OK)
		{
			var content = await tokenResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			throw new HttpRequestException(content);
		}

		var token = await tokenResponse.Content
			.ReadFromJsonAsync<Token>(_serializerOptions)
			.ConfigureAwait(false);

		return token!;
	}

	internal async Task<AccessToken> Refresh(TokenRefresh tokenRefresh)
	{
		var tokenResponse = await _httpClient
			.PostAsJsonAsync(Routes.Tokens.Refresh, tokenRefresh, _serializerOptions)
			.ConfigureAwait(false);

		if (tokenResponse.StatusCode is not HttpStatusCode.OK)
		{
			var content = await tokenResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			throw new HttpRequestException(content);
		}

		var accessToken = await tokenResponse.Content
			.ReadFromJsonAsync<AccessToken>(_serializerOptions)
			.ConfigureAwait(false);

		return accessToken!;
	}
}
