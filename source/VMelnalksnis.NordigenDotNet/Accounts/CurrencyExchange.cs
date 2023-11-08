using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>An exchange rate between currencies.</summary>
public record CurrencyExchange
{
	/// <summary>Gets or sets a ISO 4217 currency code.</summary>
	public string SourceCurrency { get; set; } = null!;

	/// <summary>Gets or sets the exchange rate.</summary>
	[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
	public decimal ExchangeRate { get; set; }
}
