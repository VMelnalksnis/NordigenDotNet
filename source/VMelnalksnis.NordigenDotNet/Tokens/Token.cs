// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Tokens;

internal class Token : AccessToken
{
	[JsonConstructor]
	public Token(string access, int accessExpires, string refresh, int refreshExpires)
		: base(access, accessExpires)
	{
		Refresh = refresh;
		RefreshExpires = refreshExpires;
	}

	public string Refresh { get; }

	[JsonPropertyName("refresh_expires")]
	public int RefreshExpires { get; }
}
