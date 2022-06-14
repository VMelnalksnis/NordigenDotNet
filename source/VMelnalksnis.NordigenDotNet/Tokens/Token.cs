// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Tokens;

internal record Token(
	string Access,
	int AccessExpires,
	string Refresh,
	[property: JsonPropertyName("refresh_expires")] int RefreshExpires) : AccessToken(Access, AccessExpires);
