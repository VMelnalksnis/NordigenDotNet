// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Tokens;

/// <summary>Handles authorization tokens for <see cref="NordigenHttpClient"/>.</summary>
public sealed class TokenDelegatingHandler : DelegatingHandler
{
	private static readonly TokenSerializationContext _context = new(new(JsonSerializerDefaults.Web));
	private static readonly JsonTypeInfo<AccessToken> _accessTokenInfo = _context.GetTypeInfo<AccessToken>();
	private static readonly JsonTypeInfo<Token> _tokenInfo = _context.GetTypeInfo<Token>();
	private static readonly JsonTypeInfo<TokenCreation> _tokenCreationInfo = _context.GetTypeInfo<TokenCreation>();
	private static readonly JsonTypeInfo<TokenRefresh> _tokenRefreshInfo = _context.GetTypeInfo<TokenRefresh>();

	private readonly HttpClient _httpClient;
	private readonly NordigenOptions _nordigenOptions;

	/// <summary>Initializes a new instance of the <see cref="TokenDelegatingHandler"/> class.</summary>
	/// <param name="httpClient">Http client configured for making requests to the Nordigen API.</param>
	/// <param name="nordigenOptions">Options for connection to the Nordigen API.</param>
	public TokenDelegatingHandler(HttpClient httpClient, NordigenOptions nordigenOptions)
	{
		_httpClient = httpClient;
		_nordigenOptions = nordigenOptions;
	}

	/// <inheritdoc />
	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		var token = await CreateNewToken().ConfigureAwait(false);
		request.Headers.Authorization = new("Bearer", token.Access);

		return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
	}

	private async Task<Token> CreateNewToken()
	{
		var tokenCreation = new TokenCreation(_nordigenOptions.SecretId, _nordigenOptions.SecretKey);
		var tokenResponse = await _httpClient
			.PostAsJsonAsync(Routes.Tokens.New, tokenCreation, _tokenCreationInfo)
			.ConfigureAwait(false);

		await tokenResponse.ThrowIfNotSuccessful().ConfigureAwait(false);
		var token = await tokenResponse.Content.ReadFromJsonAsync(_tokenInfo).ConfigureAwait(false);
		return token!;
	}

	private async Task<AccessToken> Refresh(TokenRefresh tokenRefresh)
	{
		var tokenResponse = await _httpClient
			.PostAsJsonAsync(Routes.Tokens.Refresh, tokenRefresh, _tokenRefreshInfo)
			.ConfigureAwait(false);

		await tokenResponse.ThrowIfNotSuccessful().ConfigureAwait(false);
		var accessToken = await tokenResponse.Content.ReadFromJsonAsync(_accessTokenInfo).ConfigureAwait(false);

		return accessToken!;
	}
}
