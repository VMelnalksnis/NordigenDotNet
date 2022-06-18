// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using NodaTime;

using VMelnalksnis.NordigenDotNet.Institutions;

#pragma warning disable SA1623

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>A transaction that has been booked - posted to an account on the account servicer's books.</summary>
/// <param name="TransactionAmount">The amount transferred in this transaction.</param>
/// <param name="UnstructuredInformation">Unstructured information about the transaction, usually added by the debtor.</param>
/// <param name="TransactionId">A unique transaction id created by the <see cref="Institution"/>.</param>
/// <param name="BookingDate">The date when an entry is posted to an account on the account servicer's books.</param>
public record BookedTransaction(
		AmountInCurrency TransactionAmount,
		string UnstructuredInformation,
		string TransactionId,
		LocalDate BookingDate)
	: Transaction(TransactionAmount, UnstructuredInformation)
{
	/// <summary>The name of the counterparty that sends <see cref="Transaction.TransactionAmount"/> during the transaction.</summary>
	public string? DebtorName { get; init; }

	/// <summary>The account of the counterparty that sends <see cref="Transaction.TransactionAmount"/> during the transaction.</summary>
	public TransactionAccount? DebtorAccount { get; init; }

	/// <summary>The name of the counterparty that receives <see cref="Transaction.TransactionAmount"/> during the transaction.</summary>
	public string? CreditorName { get; init; }

	/// <summary>The account of the counterparty that receives <see cref="Transaction.TransactionAmount"/> during the transaction.</summary>
	public TransactionAccount? CreditorAccount { get; init; }

	/// <summary>The ISO 20022 bank transaction code.</summary>
	/// <example>Some example values:
	/// <code>
	/// PMNT-ICDT-STDO
	/// PMNT-IRCT-STDO
	/// </code></example>
	public string? BankTransactionCode { get; init; }

	/// <summary>A transaction id, used both by the transaction and any fees paid to the <see cref="Institution"/> for the transaction.</summary>
	public string? EntryReference { get; init; }

	/// <summary>Additional structured information about the transaction from the institution.</summary>
	/// <example>
	/// <code>
	/// PURCHASE
	/// INWARD TRANSFER
	/// </code></example>
	public string? AdditionalInformation { get; init; }
}
