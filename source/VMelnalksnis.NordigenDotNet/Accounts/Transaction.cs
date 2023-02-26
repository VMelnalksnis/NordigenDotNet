// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Institutions;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>Common information for all transactions.</summary>
public abstract record Transaction
{
	/// <summary>Gets or sets a unique transaction id created by the <see cref="Institution"/>.</summary>
	public string TransactionId { get; set; } = null!;

	/// <summary>Gets or sets the amount transferred in this transaction.</summary>
	public AmountInCurrency TransactionAmount { get; set; } = null!;

	/// <summary>Gets or sets unstructured information about the transaction, usually added by the debtor.</summary>
	[JsonPropertyName("remittanceInformationUnstructured")]
	public string UnstructuredInformation { get; set; } = null!;

	/// <summary>Gets or sets structured information about the transaction.</summary>
	[JsonPropertyName("remittanceInformationStructured")]
	public string? StructuredInformation { get; set; }

	/// <summary>Gets or sets the date when the transaction was valued at.</summary>
	public LocalDate? ValueDate { get; set; }
}
