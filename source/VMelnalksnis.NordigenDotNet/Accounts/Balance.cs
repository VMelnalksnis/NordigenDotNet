// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using NodaTime;

#pragma warning disable SA1623

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>Account balance.</summary>
/// <param name="BalanceAmount">The balance amount.</param>
/// <param name="BalanceType">The balance type.</param>
/// <param name="CreditLimitIncluded">Whether the credit limit is included in <paramref name="BalanceAmount"/>.</param>
public record Balance(AmountInCurrency BalanceAmount, string BalanceType, bool CreditLimitIncluded)
{
	/// <summary>The date on which the balance was calculated.</summary>
	public LocalDate? ReferenceDate { get; init; }
}

internal record BalancesWrapper(List<Balance> Balances);
