// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>A transaction that is still pending.</summary>
/// <param name="TransactionAmount">The amount transferred in this transaction.</param>
/// <param name="UnstructuredInformation">Unstructured information about the transaction, usually added by the debtor.</param>
public record PendingTransaction(AmountInCurrency TransactionAmount, string UnstructuredInformation)
	: Transaction(TransactionAmount, UnstructuredInformation);
