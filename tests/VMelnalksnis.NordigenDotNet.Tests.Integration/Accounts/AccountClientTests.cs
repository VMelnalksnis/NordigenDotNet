// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;

using VMelnalksnis.NordigenDotNet.Accounts;
using VMelnalksnis.NordigenDotNet.Requisitions;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Accounts;

public sealed class AccountClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly IAccountClient _accountClient;
	private readonly IRequisitionClient _requisitionClient;

	public AccountClientTests(ServiceProviderFixture serviceProviderFixture)
	{
		_accountClient = serviceProviderFixture.AccountClient;
		_requisitionClient = serviceProviderFixture.RequisitionClient;
	}

	[Fact]
	public async Task Get()
	{
		var requisition = await _requisitionClient.Get().SingleAsync();

		foreach (var id in requisition.Accounts)
		{
			await _accountClient.Get(id);
			await _accountClient.GetDetails(id);
			await _accountClient.GetBalances(id);
			await _accountClient.GetTransactions(id);
		}
	}
}
