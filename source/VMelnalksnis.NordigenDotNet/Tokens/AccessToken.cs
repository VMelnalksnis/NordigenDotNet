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
