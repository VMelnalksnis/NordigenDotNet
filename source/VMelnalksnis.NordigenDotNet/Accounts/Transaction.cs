﻿using System.Text.Json.Serialization;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Institutions;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>Common information for all transactions.</summary>
public abstract record Transaction
{
	/// <summary>Gets or sets the name of the counterparty that receives <see cref="Transaction.TransactionAmount"/> during the transaction.</summary>
	public string? CreditorName { get; set; }

	/// <summary>Gets or sets the amount transferred in this transaction.</summary>
	public AmountInCurrency TransactionAmount { get; set; } = null!;

	/// <summary>Gets or sets the exchange rate used for this transaction.</summary>
	public CurrencyExchange CurrencyExchange { get; set; } = null!;

	/// <summary>Gets or sets unstructured information about the transaction, usually added by the debtor.</summary>
	[JsonPropertyName("remittanceInformationUnstructured")]
	public string UnstructuredInformation { get; set; } = null!;

	/// <summary>Gets or sets structured information about the transaction.</summary>
	[JsonPropertyName("remittanceInformationStructured")]
	public string? StructuredInformation { get; set; }

	/// <summary>Gets or sets a transaction id, used both by the transaction and any fees paid to the <see cref="Institution"/> for the transaction.</summary>
	public string? EntryReference { get; set; }

	/// <summary>Gets or sets the date when the transaction was valued at.</summary>
	public LocalDate? ValueDate { get; set; }

	/// <summary>Gets or sets the date and time when the transaction was valued at.</summary>
	public OffsetDateTime? ValueDateTime { get; set; }

	/// <summary>Gets or sets the date and time when an entry is posted to an account on the account servicer's books.</summary>
	public LocalDate? BookingDate { get; set; }

	/// <summary>Gets or sets the date when an entry is posted to an account on the account servicer's books.</summary>
	public OffsetDateTime? BookingDateTime { get; set; }

	/// <summary>
	/// Gets or sets additional information which be used by the financial institution
	/// to transport additional transaction related information.
	/// </summary>
	/// <example><code>
	/// PURCHASE
	/// INWARD TRANSFER
	/// </code></example>
	public string? AdditionalInformation { get; set; }

	/// <summary>Gets or sets merchant category code as defined by card issuer.</summary>
	public string? MerchantCategoryCode { get; set; }

	/// <summary>Gets or sets bank transaction code as used by the financial institution, defined by ISO20022.</summary>
	/// <example>Some example values:
	/// <code>
	/// PMNT-ICDT-STDO
	/// PMNT-IRCT-STDO
	/// </code></example>
	public string? BankTransactionCode { get; set; }
}
