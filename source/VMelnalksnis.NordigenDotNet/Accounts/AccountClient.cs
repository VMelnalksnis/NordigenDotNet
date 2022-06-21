// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <inheritdoc />
public sealed class AccountClient : IAccountClient
{
	private readonly NordigenHttpClient _nordigenHttpClient;

	/// <summary>Initializes a new instance of the <see cref="AccountClient"/> class.</summary>
	/// <param name="nordigenHttpClient">Http client configured for making requests to the Nordigen API.</param>
	public AccountClient(NordigenHttpClient nordigenHttpClient)
	{
		_nordigenHttpClient = nordigenHttpClient;
	}

	/// <inheritdoc />
	public async Task<Account> Get(Guid id, CancellationToken cancellationToken = default)
	{
		var account = await _nordigenHttpClient
			.GetAsJson<Account>(Routes.Accounts.IdUri(id), cancellationToken)
			.ConfigureAwait(false);

		return account!;
	}

	/// <inheritdoc />
	public async Task<List<Balance>> GetBalances(Guid id, CancellationToken cancellationToken = default)
	{
		var balances = await _nordigenHttpClient
			.GetAsJson<BalancesWrapper>(Routes.Accounts.BalancesUri(id), cancellationToken)
			.ConfigureAwait(false);

		return balances!.Balances;
	}

	/// <inheritdoc />
	public async Task<AccountDetails> GetDetails(Guid id, CancellationToken cancellationToken = default)
	{
		var details = await _nordigenHttpClient
			.GetAsJson<AccountDetailsWrapper>(Routes.Accounts.DetailsUri(id), cancellationToken)
			.ConfigureAwait(false);

		return details!.Account;
	}

	/// <inheritdoc />
	public async Task<Transactions> GetTransactions(
		Guid id,
		Interval? interval = null,
		CancellationToken cancellationToken = default)
	{
		var uri = Routes.Accounts.TransactionsUri(id, interval);

		var transactions = await _nordigenHttpClient
			.GetAsJson<TransactionsWrapper>(uri, cancellationToken)
			.ConfigureAwait(false);

		return transactions!.Transactions;
	}
}
