// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>Additional details of an account.</summary>
public record AccountDetails
{
	/// <summary>Gets or sets the account id of the given account in the financial institution.</summary>
	public string ResourceId { get; set; } = null!;

	/// <summary>Gets or sets the IBAN of the account.</summary>
	public string? Iban { get; set; }

	/// <summary>Gets or sets a ISO 4217 currency code.</summary>
	public string Currency { get; set; } = null!;

	/// <summary>
	/// Gets or sets the name of the legal account owner.
	/// If there is more than one owner, then e.g. two names might be noted here.
	/// For a corporate account, the corporate name is used for this attribute.</summary>
	public string? OwnerName { get; set; }

	/// <summary>Gets or sets the name of the account, as assigned by the financial institution.</summary>
	public string? Name { get; set; }

	/// <summary>Gets or sets product Name of the Bank for this account, proprietary definition.</summary>
	public string? Product { get; set; }

	/// <summary>Gets or sets externalCashAccountType1Code from ISO 20022.</summary>
	public string? CashAccountType { get; set; }
}

#pragma warning disable SA1402
internal class AccountDetailsWrapper
{
	public AccountDetails Account { get; set; } = null!;
}
