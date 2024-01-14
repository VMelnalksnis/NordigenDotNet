using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Tokens;

/// <inheritdoc cref="JsonSerializerContext" />
[JsonSerializable(typeof(AccessToken))]
[JsonSerializable(typeof(Token))]
[JsonSerializable(typeof(TokenCreation))]
[JsonSerializable(typeof(TokenRefresh))]
internal partial class TokenSerializationContext : JsonSerializerContext
{
}
