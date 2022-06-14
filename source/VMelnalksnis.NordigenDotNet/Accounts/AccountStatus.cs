// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>All possible states of an account.</summary>
public enum AccountStatus
{
	/// <summary>User has successfully authenticated and account is discovered.</summary>
	Discovered = 1,

	/// <summary>Account is being processed by the Institution.</summary>
	Processing,

	/// <summary>An error was encountered when processing account.</summary>
	Error,

	/// <summary>Access to account has expired as set in End User Agreement.</summary>
	Expired,

	/// <summary>Account has been successfully processed.</summary>
	Ready,

	/// <summary>Account has been suspended (more than 10 consecutive failed attempts to access the account).</summary>
	Suspended,
}
