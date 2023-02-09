// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Requisitions;

using Xunit.Abstractions;

using static VMelnalksnis.NordigenDotNet.Tests.Integration.ServiceProviderFixture;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Accounts;

public sealed class AccountClientTests : IClassFixture<ServiceProviderFixture>, IAsyncLifetime
{
	private readonly ITestOutputHelper _testOutputHelper;
	private readonly INordigenClient _nordigenClient;
	private Requisition _requisition = null!;
	private Guid _accountId;

	public AccountClientTests(ITestOutputHelper testOutputHelper, ServiceProviderFixture serviceProviderFixture)
	{
		_testOutputHelper = testOutputHelper;
		_nordigenClient = serviceProviderFixture.GetNordigenClient(testOutputHelper);
	}

	public async Task InitializeAsync()
	{
		_requisition = await GetRequisition();
		_accountId = _requisition.Accounts.OrderBy(guid => guid).First();
	}

	[Fact]
	public async Task Get_ShouldReturnExpected()
	{
		_requisition.Accounts.Should().HaveCount(2);

		var account = await _nordigenClient.Accounts.Get(_accountId);
		var currentInstant = SystemClock.Instance.GetCurrentInstant();
		using (new AssertionScope())
		{
			account.Id.Should().Be(_accountId);
			account.Created.Should().BeLessThan(account.LastAccessed.GetValueOrDefault()).And.BeLessThan(currentInstant);
			account.LastAccessed.Should().NotBeNull();
			account.LastAccessed.GetValueOrDefault().Should().BeGreaterThan(currentInstant - Duration.FromMinutes(1));
			account.Iban.Should().Be("GL3343697694912188");
			account.InstitutionId.Should().Be(IntegrationInstitutionId);
			account.Status.Should().BeDefined();
		}
	}

	[Fact]
	public async Task GetDetails_ShouldReturnExpected()
	{
		var accountDetails = await _nordigenClient.Accounts.GetDetails(_accountId);

		using (new AssertionScope())
		{
			accountDetails.ResourceId.Should().Be("01F3NS4YV94RA29YCH8R0F6BMF");
			accountDetails.Iban.Should().Be("GL3343697694912188");
			accountDetails.Currency.Should().Be("EUR");
			accountDetails.OwnerName.Should().Be("John Doe");
			accountDetails.Name.Should().Be("Main Account");
			accountDetails.Product.Should().Be("Checkings");
			accountDetails.CashAccountType.Should().Be("CACC");
		}
	}

	[Fact]
	public async Task GetBalances_ShouldReturnExpected()
	{
		var balances = await _nordigenClient.Accounts.GetBalances(_accountId);

		balances.Should().HaveCount(2);

		var firstBalance = balances.First();
		var secondBalance = balances.Last();
		using (new AssertionScope())
		{
			firstBalance.Should().BeEquivalentTo(secondBalance, options => options.Excluding(balance => balance.BalanceType));
			firstBalance.BalanceType.Should().Be("expected");
			secondBalance.BalanceType.Should().Be("interimAvailable");
		}
	}

	[Fact]
	public async Task GetTransactions_ShouldReturnExpected()
	{
		var currentInstant = SystemClock.Instance.GetCurrentInstant();
		var currentZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
		var currentDate = currentInstant.InZone(currentZone).Date;

		var dateTo = currentInstant;
		var dateFrom = dateTo - Duration.FromDays(7);
		var transactions = await _nordigenClient.Accounts.GetTransactions(_accountId, new Interval(dateFrom, dateTo));
		var allTransactions = await _nordigenClient.Accounts.GetTransactions(_accountId);

		using (new AssertionScope())
		{
			allTransactions.Booked.Should().HaveCount(12);
			transactions.Booked.Should().HaveCount(12).And.BeSubsetOf(allTransactions.Booked);
			transactions.Pending.Should().BeEquivalentTo(allTransactions.Pending);

			var pendingTransaction = transactions.Pending.Should().ContainSingle().Subject;
			pendingTransaction.TransactionAmount.Currency.Should().Be("EUR");
			pendingTransaction.TransactionAmount.Amount.Should().Be(10m);
			pendingTransaction.UnstructuredInformation.Should().Be("Reserved PAYMENT Emperor's Burgers");
			pendingTransaction.ValueDate.Should().Be(currentDate - Period.FromDays(2));

			var date = currentDate - Period.FromDays(1);
			var transactionId = $"{date:yyyyMMdd}01927908-1";
			var bookedTransaction = transactions.Booked.First(transaction => transaction.TransactionId == transactionId);
			bookedTransaction.TransactionAmount.Currency.Should().Be("EUR");
			bookedTransaction.TransactionAmount.Amount.Should().Be(-15);
			bookedTransaction.UnstructuredInformation.Should().Be("PAYMENT Alderaan Coffe");
			bookedTransaction.ValueDate.Should().Be(date);
			bookedTransaction.TransactionId.Should().Be(transactionId);
			bookedTransaction.BookingDate.Should().Be(date);
			bookedTransaction.DebtorName.Should().BeNull();
			bookedTransaction.DebtorAccount.Should().BeNull();
			bookedTransaction.CreditorName.Should().BeNull();
			bookedTransaction.CreditorAccount.Should().BeNull();
			bookedTransaction.BankTransactionCode.Should().Be("PMNT");
			bookedTransaction.EntryReference.Should().BeNull();
			bookedTransaction.AdditionalInformation.Should().BeNull();
		}
	}

	public Task DisposeAsync() => Task.CompletedTask;

	private async Task<Requisition> GetRequisition()
	{
		var requisition = await _nordigenClient.Requisitions.Get().SingleOrDefaultAsync(r =>
			r is { InstitutionId: IntegrationInstitutionId, Status: RequisitionStatus.Ln });

		if (requisition is not null)
		{
			return requisition;
		}

		var creation = new RequisitionCreation(new("https://github.com/VMelnalksnis/NordigenDotNet"), IntegrationInstitutionId);
		var createdRequisition = await _nordigenClient.Requisitions.Post(creation);

		var message = $@"Access to an institution needs to be approved every 90 days.
Please go to {createdRequisition.Link} and rerun this test.
For more information see https://nordigen.com/en/account_information_documenation/integration/sandbox/";

		_testOutputHelper.WriteLine(message);
		throw new(message);
	}
}
