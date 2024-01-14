using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>An amount in a specific currency.</summary>
public record AmountInCurrency
{
	/// <summary>Gets or sets a ISO 4217 currency code.</summary>
	public string Currency { get; set; } = null!;

	/// <summary>Gets or sets the amount.</summary>
	[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
	public decimal Amount { get; set; }
}
