using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>Nordigen API client for reading account information.</summary>
public interface IAccountClient
{
	/// <summary>Gets account metadata.</summary>
	/// <param name="id">The id of the account.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The account with the specified id.</returns>
	Task<Account> Get(Guid id, CancellationToken cancellationToken = default);

	/// <summary>Gets additional details about the account.</summary>
	/// <param name="id">The id of the account.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The details of the account with the specified id.</returns>
	Task<AccountDetails> GetDetails(Guid id, CancellationToken cancellationToken = default);

	/// <summary>Gets current account balances.</summary>
	/// <param name="id">The id of the account.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The balances of the account with the specified id.</returns>
	Task<List<Balance>> GetBalances(Guid id, CancellationToken cancellationToken = default);

	/// <summary>Gets all transactions for the account within the specified interval.</summary>
	/// <param name="id">The id of the account.</param>
	/// <param name="interval">The interval for which to get the transactions.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>All transactions for the specified account and interval.</returns>
	Task<Transactions> GetTransactions(Guid id, Interval? interval = null, CancellationToken cancellationToken = default);
}
