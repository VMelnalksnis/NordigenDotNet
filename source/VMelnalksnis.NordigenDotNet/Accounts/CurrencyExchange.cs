using System.Text.Json.Serialization;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>An exchange rate between currencies.</summary>
public record CurrencyExchange
{
	/// <summary>Gets or sets a ISO 4217 currency code of the source currency.</summary>
	public string SourceCurrency { get; set; } = null!;

	/// <summary>Gets or sets the exchange rate.</summary>
	[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
	public decimal ExchangeRate { get; set; }

	/// <summary>Gets or sets a ISO 4217 currency code of the unit currency of <see cref="ExchangeRate"/>.</summary>
	public string? UnitCurrency { get; set; }

	/// <summary>Gets or sets a ISO 4217 currency code of the target currency.</summary>
	public string? TargetCurrency { get; set; }

	/// <summary>Gets or sets the date when the <see cref="ExchangeRate"/> was quoted.</summary>
	public LocalDate? QuotationDate { get; set; }

	/// <summary>Gets or sets the current exchange contract id.</summary>
	public string? ContractIdentification { get; set; }
}
