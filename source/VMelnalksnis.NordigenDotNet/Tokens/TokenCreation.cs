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
