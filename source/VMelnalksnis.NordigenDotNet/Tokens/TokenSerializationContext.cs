// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Tokens;

/// <inheritdoc cref="JsonSerializerContext" />
[JsonSerializable(typeof(AccessToken))]
[JsonSerializable(typeof(Token))]
[JsonSerializable(typeof(TokenCreation))]
[JsonSerializable(typeof(TokenRefresh))]
internal partial class TokenSerializationContext : JsonSerializerContext;
