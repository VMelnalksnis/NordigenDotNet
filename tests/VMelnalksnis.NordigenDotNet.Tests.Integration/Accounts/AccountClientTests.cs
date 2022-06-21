// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Accounts;

public sealed class AccountClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly INordigenClient _nordigenClient;

	public AccountClientTests(ServiceProviderFixture serviceProviderFixture)
	{
		_nordigenClient = serviceProviderFixture.NordigenClient;
	}

	[Fact(Skip = "Requires manual setup at least every 90 days")]
	public async Task Get()
	{
		var requisition = await _nordigenClient.Requisitions.Get().SingleAsync();

		foreach (var id in requisition.Accounts)
		{
			await _nordigenClient.Accounts.Get(id);
			await _nordigenClient.Accounts.GetDetails(id);
			await _nordigenClient.Accounts.GetBalances(id);

			var dateTo = SystemClock.Instance.GetCurrentInstant();
			var dateFrom = dateTo - Duration.FromDays(7);
			var transactions = await _nordigenClient.Accounts.GetTransactions(id, new Interval(dateFrom, dateTo));
			var allTransactions = await _nordigenClient.Accounts.GetTransactions(id);

			transactions.Booked.Should().BeSubsetOf(allTransactions.Booked);
		}
	}
}
