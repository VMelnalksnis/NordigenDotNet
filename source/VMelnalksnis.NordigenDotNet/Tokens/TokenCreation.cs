// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Tokens;

internal sealed class TokenCreation
{
	[JsonConstructor]
	public TokenCreation(string secretId, string secretKey)
	{
		SecretId = secretId;
		SecretKey = secretKey;
	}

	[JsonPropertyName("secret_id")]
	public string SecretId { get; }

	[JsonPropertyName("secret_key")]
	public string SecretKey { get; }
}
