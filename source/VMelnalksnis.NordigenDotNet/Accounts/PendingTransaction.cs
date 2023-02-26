// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using VMelnalksnis.NordigenDotNet.Institutions;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>A transaction that is still pending.</summary>
public record PendingTransaction : Transaction
{
	/// <summary>Gets or sets a unique transaction id created by the <see cref="Institution"/>.</summary>
	public string? TransactionId { get; set; }
}
