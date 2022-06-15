// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Tokens;

internal class AccessToken
{
	[JsonConstructor]
	public AccessToken(string access, int accessExpires)
	{
		Access = access;
		AccessExpires = accessExpires;
	}

	public string Access { get; }

	[JsonPropertyName("access_expires")]
	public int AccessExpires { get; }
}
