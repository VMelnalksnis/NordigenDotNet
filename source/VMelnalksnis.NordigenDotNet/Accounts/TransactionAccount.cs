// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>An account that can be referenced from a <see cref="Transaction"/>.</summary>
public record TransactionAccount
{
	/// <summary>Gets or sets the IBAN of the account.</summary>
	public string Iban { get; set; } = null!;
}
