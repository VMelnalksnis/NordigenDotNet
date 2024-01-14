using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Tokens;

internal sealed class TokenRefresh
{
	[JsonConstructor]
	public TokenRefresh(string refresh)
	{
		Refresh = refresh;
	}

	public string Refresh { get; }
}
