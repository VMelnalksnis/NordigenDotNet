// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>Additional details of an account.</summary>
public record AccountDetails(
	string ResourceId,
	string Iban,
	string Currency,
	string OwnerName,
	string Name,
	string Product,
	string CashAccountType);

internal record AccountDetailsWrapper(AccountDetails Account);
