// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Tokens;

internal sealed class TokenClient
{
	private static readonly TokenSerializationContext _context = new(new(JsonSerializerDefaults.Web));
	private static readonly JsonTypeInfo<AccessToken> _accessTokenInfo = (JsonTypeInfo<AccessToken>)_context.GetTypeInfo(typeof(AccessToken));
	private static readonly JsonTypeInfo<Token> _tokenInfo = (JsonTypeInfo<Token>)_context.GetTypeInfo(typeof(Token));
	private static readonly JsonTypeInfo<TokenCreation> _tokenCreationInfo = (JsonTypeInfo<TokenCreation>)_context.GetTypeInfo(typeof(TokenCreation));
	private static readonly JsonTypeInfo<TokenRefresh> _tokenRefreshInfo = (JsonTypeInfo<TokenRefresh>)_context.GetTypeInfo(typeof(TokenRefresh));

	private readonly HttpClient _httpClient;

	internal TokenClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	internal async Task<Token> New(TokenCreation tokenCreation)
	{
		var tokenResponse = await _httpClient
			.PostAsJsonAsync(Routes.Tokens.New, tokenCreation, _tokenCreationInfo)
			.ConfigureAwait(false);

		await tokenResponse.ThrowIfNotSuccessful().ConfigureAwait(false);
		var token = await tokenResponse.Content.ReadFromJsonAsync(_tokenInfo).ConfigureAwait(false);
		return token!;
	}

	internal async Task<AccessToken> Refresh(TokenRefresh tokenRefresh)
	{
		var tokenResponse = await _httpClient
			.PostAsJsonAsync(Routes.Tokens.Refresh, tokenRefresh, _tokenRefreshInfo)
			.ConfigureAwait(false);

		await tokenResponse.ThrowIfNotSuccessful().ConfigureAwait(false);
		var accessToken = await tokenResponse.Content.ReadFromJsonAsync(_accessTokenInfo).ConfigureAwait(false);

		return accessToken!;
	}
}
