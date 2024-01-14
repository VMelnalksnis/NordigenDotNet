using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Institutions;

/// <summary>Account servicing payment service provider details.</summary>
public record Institution
{
	/// <summary>Gets or sets the id of the service provider.</summary>
	public string Id { get; set; } = null!;

	/// <summary>Gets or sets the name of the service provider.</summary>
	public string Name { get; set; } = null!;

	/// <summary>Gets or sets the BIC of the service provider.</summary>
	public string Bic { get; set; } = null!;

	/// <summary>Gets or sets the maximum number of days in the past the service provider will return transactions for.</summary>
	[JsonPropertyName("transaction_total_days")]
	[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
	public int TransactionTotalDays { get; set; }

	/// <summary>Gets or sets all the countries supported by the service provider.</summary>
	public List<string> Countries { get; set; } = null!;

	/// <summary>Gets or sets uRI to the service provider's logo.</summary>
	public Uri Logo { get; set; } = null!;
}
