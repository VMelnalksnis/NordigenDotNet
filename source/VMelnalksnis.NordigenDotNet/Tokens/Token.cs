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
