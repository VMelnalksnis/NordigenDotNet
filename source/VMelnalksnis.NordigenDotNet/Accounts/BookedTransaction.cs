// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Accounts;

public record BookedTransaction(
		AmountInCurrency Amount,
		string RemittanceInformationUnstructured,
		string TransactionId,
		LocalDate BookingDate)
	: Transaction(Amount, RemittanceInformationUnstructured)
{
	public string? DebtorName { get; init; }

	public DebtorAccount? DebtorAccount { get; init; }

	public string? CreditorName { get; init; }

	public DebtorAccount? CreditorAccount { get; init; }

	public string? BankTransactionCode { get; init; }

	public string? EntryReference { get; init; }

	public string? AdditionalInformation { get; init; }
}
