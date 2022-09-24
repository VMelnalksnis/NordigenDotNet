// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Requisitions;

using Xunit.Abstractions;

using static VMelnalksnis.NordigenDotNet.Tests.Integration.ServiceProviderFixture;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Accounts;

public sealed class AccountClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly ITestOutputHelper _testOutputHelper;
	private readonly INordigenClient _nordigenClient;
	private readonly Requisition _requisition;

	public AccountClientTests(ITestOutputHelper testOutputHelper, ServiceProviderFixture serviceProviderFixture)
	{
		_testOutputHelper = testOutputHelper;
		_nordigenClient = serviceProviderFixture.GetNordigenClient(testOutputHelper);

		_requisition = GetRequisition().GetAwaiter().GetResult();
	}

	[Fact]
	public async Task Get_ShouldReturnExpected()
	{
		_requisition.Accounts.Should().HaveCount(2);
		var id = _requisition.Accounts.First();

		var account = await _nordigenClient.Accounts.Get(id);
		var currentInstant = SystemClock.Instance.GetCurrentInstant();
		using (new AssertionScope())
		{
			account.Id.Should().Be(id);
			account.Created.Should().BeLessThan(account.LastAccessed).And.BeLessThan(currentInstant);
			account.LastAccessed.Should().BeGreaterThan(currentInstant - Duration.FromMinutes(1));
			account.Iban.Should().Be("GL3343697694912188");
			account.InstitutionId.Should().Be(IntegrationInstitutionId);
			account.Status.Should().BeDefined();
		}
	}

	[Fact]
	public async Task GetDetails_ShouldReturnExpected()
	{
		var id = _requisition.Accounts.First();

		var accountDetails = await _nordigenClient.Accounts.GetDetails(id);

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
		var id = _requisition.Accounts.First();

		var balances = await _nordigenClient.Accounts.GetBalances(id);

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
		var id = _requisition.Accounts.First();

		var currentInstant = SystemClock.Instance.GetCurrentInstant();
		var currentZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
		var currentDate = currentInstant.InZone(currentZone).Date;

		var dateTo = currentInstant;
		var dateFrom = dateTo - Duration.FromDays(7);
		var transactions = await _nordigenClient.Accounts.GetTransactions(id, new Interval(dateFrom, dateTo));
		var allTransactions = await _nordigenClient.Accounts.GetTransactions(id);

		using (new AssertionScope())
		{
			allTransactions.Booked.Should().HaveCount(534);
			transactions.Booked.Should().HaveCount(42).And.BeSubsetOf(allTransactions.Booked);
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

	private async Task<Requisition> GetRequisition()
	{
		var requisition = await _nordigenClient.Requisitions.Get().SingleOrDefaultAsync(r =>
			r.InstitutionId is IntegrationInstitutionId &&
			r.Status is RequisitionStatus.Ln);

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
