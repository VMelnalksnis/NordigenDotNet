// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>All transactions of an account.</summary>
public record Transactions
{
	/// <summary>Gets or sets all transactions that have been booked.</summary>
	public List<BookedTransaction> Booked { get; set; } = null!;

	/// <summary>Gets or sets all transaction that are still pending.</summary>
	public List<PendingTransaction> Pending { get; set; } = null!;
}

internal class TransactionsWrapper
{
	public Transactions Transactions { get; set; } = null!;
}
