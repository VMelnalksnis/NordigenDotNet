// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Tokens;

/// <summary>Handles authorization tokens for <see cref="NordigenHttpClient"/>.</summary>
public sealed class TokenDelegatingHandler : DelegatingHandler
{
	private static readonly TokenSerializationContext _context = new(new(JsonSerializerDefaults.Web));

	private readonly HttpClient _httpClient;
	private readonly NordigenOptions _nordigenOptions;
	private readonly NordigenTokenCache _tokenCache;

	/// <summary>Initializes a new instance of the <see cref="TokenDelegatingHandler"/> class.</summary>
	/// <param name="httpClient">Http client configured for making requests to the Nordigen API.</param>
	/// <param name="nordigenOptions">Options for connection to the Nordigen API.</param>
	/// <param name="tokenCache">Nordigen API token cache for preserving tokens between requests.</param>
	public TokenDelegatingHandler(HttpClient httpClient, NordigenOptions nordigenOptions, NordigenTokenCache tokenCache)
	{
		_httpClient = httpClient;
		_nordigenOptions = nordigenOptions;
		_tokenCache = tokenCache;
	}

	/// <inheritdoc />
	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		if (_tokenCache.AccessToken is null || _tokenCache.IsRefreshExpired)
		{
			await CreateNewToken().ConfigureAwait(false);
		}
		else if (_tokenCache.IsAccessExpired)
		{
			await RefreshToken().ConfigureAwait(false);
		}

		request.Headers.Authorization = new("Bearer", _tokenCache.AccessToken?.Access);
		var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
		if (response.StatusCode is not HttpStatusCode.Unauthorized)
		{
			return response;
		}

		await CreateNewToken().ConfigureAwait(false);
		return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
	}

	private async Task CreateNewToken()
	{
		var tokenCreation = new TokenCreation(_nordigenOptions.SecretId, _nordigenOptions.SecretKey);
		var tokenResponse = await _httpClient
			.PostAsJsonAsync(Routes.Tokens.New, tokenCreation, _context.TokenCreation)
			.ConfigureAwait(false);

		await tokenResponse.ThrowIfNotSuccessful().ConfigureAwait(false);
		var token = await tokenResponse.Content.ReadFromJsonAsync(_context.Token).ConfigureAwait(false);
		_tokenCache.SetToken(token!);
	}

	private async Task RefreshToken()
	{
		var tokenResponse = await _httpClient
			.PostAsJsonAsync(Routes.Tokens.Refresh, new(_tokenCache.Token!.Refresh), _context.TokenRefresh)
			.ConfigureAwait(false);

		await tokenResponse.ThrowIfNotSuccessful().ConfigureAwait(false);
		var accessToken = await tokenResponse.Content.ReadFromJsonAsync(_context.AccessToken).ConfigureAwait(false);

		_tokenCache.SetAccessToken(accessToken!);
	}
}
