// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>All transactions of an account.</summary>
/// <param name="Booked">All transactions that have been booked.</param>
/// <param name="Pending">All transaction that are still pending.</param>
public record Transactions(List<BookedTransaction> Booked, List<PendingTransaction> Pending);

internal record TransactionsWrapper(Transactions Transactions);
