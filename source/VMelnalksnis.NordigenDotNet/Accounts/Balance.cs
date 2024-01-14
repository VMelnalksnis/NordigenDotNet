using System.Collections.Generic;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>Account balance.</summary>
public record Balance
{
	/// <summary>Gets or sets the balance amount.</summary>
	public AmountInCurrency BalanceAmount { get; set; } = null!;

	/// <summary>Gets or sets the balance type.</summary>
	public string BalanceType { get; set; } = null!;

	/// <summary>Gets or sets a value indicating whether whether the credit limit is included in <see cref="BalanceAmount"/>.</summary>
	public bool CreditLimitIncluded { get; set; }

	/// <summary>Gets or sets the date on which the balance was calculated.</summary>
	public LocalDate? ReferenceDate { get; set; }
}

#pragma warning disable SA1402
internal class BalancesWrapper
{
	public List<Balance> Balances { get; set; } = null!;
}
