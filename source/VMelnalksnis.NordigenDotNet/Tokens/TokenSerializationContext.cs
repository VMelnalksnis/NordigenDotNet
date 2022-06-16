// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace VMelnalksnis.NordigenDotNet.Tokens;

/// <inheritdoc />
[JsonSerializable(typeof(AccessToken))]
[JsonSerializable(typeof(Token))]
[JsonSerializable(typeof(TokenCreation))]
[JsonSerializable(typeof(TokenRefresh))]
internal partial class TokenSerializationContext : JsonSerializerContext
{
	internal JsonTypeInfo<T> GetTypeInfo<T>() => (JsonTypeInfo<T>)GetTypeInfo(typeof(T));
}
