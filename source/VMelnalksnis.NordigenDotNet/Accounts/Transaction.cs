// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

using NodaTime;

#pragma warning disable SA1623

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>Common information for all transactions.</summary>
/// <param name="TransactionAmount">The amount transferred in this transaction.</param>
/// <param name="UnstructuredInformation">Unstructured information about the transaction, usually added by the debtor.</param>
public abstract record Transaction(
	AmountInCurrency TransactionAmount,
	[property: JsonPropertyName("remittanceInformationUnstructured")] string UnstructuredInformation)
{
	/// <summary>The date when the transaction was valued at.</summary>
	public LocalDate? ValueDate { get; init; }
}
