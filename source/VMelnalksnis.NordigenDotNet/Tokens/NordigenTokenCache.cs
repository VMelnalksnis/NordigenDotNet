// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Tokens;

/// <summary>Caches refresh and access tokens for <see cref="NordigenHttpClient"/>.</summary>
public sealed class NordigenTokenCache
{
	private readonly NordigenOptions _nordigenOptions;
	private readonly IClock _clock;

	/// <summary>Initializes a new instance of the <see cref="NordigenTokenCache"/> class.</summary>
	/// <param name="nordigenOptions">Options for connection to the Nordigen API.</param>
	/// <param name="clock">Clock for accessing the current time.</param>
	public NordigenTokenCache(NordigenOptions nordigenOptions, IClock clock)
	{
		_nordigenOptions = nordigenOptions;
		_clock = clock;
	}

	internal AccessToken? AccessToken { get; private set; }

	internal bool IsAccessExpired =>
		AccessToken is not null &&
		AccessExpiresAt is not null &&
		_clock.GetCurrentInstant() > AccessExpiresAt;

	internal Token? Token { get; private set; }

	internal bool IsRefreshExpired =>
		Token is not null &&
		RefreshExpiresAt is not null &&
		_clock.GetCurrentInstant() > RefreshExpiresAt;

	private Instant? AccessExpiresAt { get; set; }

	private Instant? RefreshExpiresAt { get; set; }

	internal void SetToken(Token token)
	{
		Token = token;
		AccessToken = token;
		var currentInstant = _clock.GetCurrentInstant();
		var factor = _nordigenOptions.ExpirationFactor;
		AccessExpiresAt = currentInstant + Duration.FromSeconds(token.AccessExpires / factor);
		RefreshExpiresAt = currentInstant + Duration.FromSeconds(token.RefreshExpires / factor);
	}

	internal void SetAccessToken(AccessToken accessToken)
	{
		AccessToken = accessToken;
		var currentInstant = _clock.GetCurrentInstant();
		var factor = _nordigenOptions.ExpirationFactor;
		AccessExpiresAt = currentInstant + Duration.FromSeconds(accessToken.AccessExpires / factor);
	}
}
