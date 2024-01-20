using System;
using System.Net.Http;
using System.Threading.Tasks;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Accounts;
using VMelnalksnis.NordigenDotNet.Tests.MockHttp;
using VMelnalksnis.NordigenDotNet.Tests.Stubs.Accounts;

namespace VMelnalksnis.NordigenDotNet.Tests.Accounts;

public sealed class AccountClientTests
{
	[Fact]
	public async Task GetDetails_ShouldReturnExpected()
	{
		var data = TestData.GetDetails.Result;
		var expected = new AccountDetails
		{
			CashAccountType = "OTHR",
			Currency = "EUR",
			Details = "A Sample Card Type",
			Iban = "GL5117350000017350",
			MaskedPan = "XXXX-XXXXXX-12345",
			Name = "Credit Account",
			OwnerName = "John Doe",
			Product = "Card",
			ResourceId = "01F3NS4YV94RA29YCH8R0F6BMF",
		};

		var accountClient = new AccountClient(
			MockHttpHandler(data),
			new(DateTimeZoneProviders.Tzdb));

		var details = await accountClient.GetDetails(Guid.NewGuid());
		details.Should().Be(expected);
	}

	[Fact]
	public async Task GetTransactions_ShouldReturnExpected()
	{
		var data = TestData.GetTransactions.Result;
		var bookingDate = Instant.FromUtc(2023, 11, 8, 10, 05);
		var valueDate = Instant.FromUtc(2023, 11, 10, 12, 12, 12);

		var bankTransactionPending = new PendingTransaction()
		{
			AdditionalInformation = "Coffee",
			BankTransactionCode = "PMNT",
			CreditorName = "Alderaan Coffee",
			CurrencyExchange = new()
			{
				ExchangeRate = decimal.Parse("0.00"),
				SourceCurrency = "GBP",
			},
			EntryReference = "2023111101697308",
			MerchantCategoryCode = "123",
			StructuredInformation = "Structured Alderaan - Coffee - Alderaan",
			TransactionAmount = new()
			{
				Currency = "GBP",
				Amount = decimal.Parse("-10.00"),
			},
			TransactionId = "2023111101697308-1",
			UnstructuredInformation = "Alderaan Coffee - Alderaan",
			BookingDate = bookingDate.InZone(DateTimeZone.Utc).Date,
			BookingDateTime = bookingDate.WithOffset(Offset.Zero),
		};

		var bankTransactionWithZuluTime = new BookedTransaction()
		{
			AdditionalInformation = "Coffee",
			BankTransactionCode = "PMNT",
			CreditorName = "Alderaan Coffee",
			CurrencyExchange = new()
			{
				ExchangeRate = decimal.Parse("0.00"),
				SourceCurrency = "GBP",
			},
			EntryReference = "2023111101697308",
			MerchantCategoryCode = "123",
			StructuredInformation = "Structured Alderaan - Coffee - Alderaan",
			TransactionAmount = new()
			{
				Currency = "GBP",
				Amount = decimal.Parse("-10.00"),
			},
			UnstructuredInformation = "Alderaan Coffee - Alderaan",
			TransactionId = "2023111101697308-1",
			BookingDate = bookingDate.InZone(DateTimeZone.Utc).Date,
			BookingDateTime = bookingDate.WithOffset(Offset.Zero),
			ValueDate = valueDate.InZone(DateTimeZone.Utc).Date,
			ValueDateTime = valueDate.WithOffset(Offset.Zero),
		};

		var bankTransactionWithZeroOffset = new BookedTransaction()
		{
			AdditionalInformation = "Coffee",
			BankTransactionCode = "PMNT",
			CreditorName = "Alderaan Coffee",
			CurrencyExchange = new()
			{
				ExchangeRate = decimal.Parse("0.00"),
				SourceCurrency = "GBP",
			},
			EntryReference = "2023111101697308",
			MerchantCategoryCode = "123",
			StructuredInformation = "Structured Alderaan - Coffee - Alderaan",
			TransactionAmount = new()
			{
				Currency = "GBP",
				Amount = decimal.Parse("-10.00"),
			},
			UnstructuredInformation = "Alderaan Coffee - Alderaan",
			BookingDate = bookingDate.InZone(DateTimeZone.Utc).Date,
			BookingDateTime = bookingDate.WithOffset(Offset.Zero),
			ValueDate = valueDate.InZone(DateTimeZone.Utc).Date,
			ValueDateTime = valueDate.WithOffset(Offset.Zero),
			TransactionId = "2023111101697308-2",
		};

		var bankTransactionWith1HourOffset = new BookedTransaction()
		{
			AdditionalInformation = "Coffee",
			BankTransactionCode = "PMNT",
			CreditorName = "Alderaan Coffee",
			CurrencyExchange = new()
			{
				ExchangeRate = decimal.Parse("0.00"),
				SourceCurrency = "GBP",
			},
			EntryReference = "2023111101697308",
			MerchantCategoryCode = "123",
			StructuredInformation = "Structured Alderaan - Coffee - Alderaan",
			TransactionAmount = new()
			{
				Currency = "GBP",
				Amount = decimal.Parse("-10.00"),
			},
			UnstructuredInformation = "Alderaan Coffee - Alderaan",
			TransactionId = "2023111101697308-3",
			BookingDate = bookingDate.InZone(DateTimeZone.Utc).Date,
			BookingDateTime = bookingDate.WithOffset(Offset.FromHours(1)).PlusHours(-1),
			ValueDate = valueDate.InZone(DateTimeZone.Utc).Date,
			ValueDateTime = valueDate.WithOffset(Offset.FromHours(1)).PlusHours(-1),
		};

		var expected = new Transactions
		{
			Booked = new()
			{
				bankTransactionWithZuluTime,
				bankTransactionWithZeroOffset,
				bankTransactionWith1HourOffset,
			},
			Pending = new()
			{
				bankTransactionPending,
			},
		};

		var accountClient = new AccountClient(
			MockHttpHandler(data),
			new(DateTimeZoneProviders.Tzdb));
		var details = await accountClient.GetTransactions(
			Guid.Parse("af9d4aa3-5520-437b-b3c8-006ae7c908e8"));
		details.Should().BeEquivalentTo(expected);
	}

	private HttpClient MockHttpHandler(string json)
	{
		var client = new HttpClient(new MockHttpMessageHandler(json));
		client.BaseAddress = new("http://localhost");
		return client;
	}
}
