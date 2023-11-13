using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Accounts;
using VMelnalksnis.NordigenDotNet.Tests.MockHttpResponse;

namespace VMelnalksnis.NordigenDotNet.Tests.Accounts;

public sealed class AccountClientTests
{
	public HttpClient MockHttpHandler(string json)
	{
		var client = new HttpClient(new HttpMessageHandlerMock(json));
		client.BaseAddress = new("http://localhost");
		return client;
	}

	public string GetTestData(string testPath)
	{
		var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"../../../Stubs/{testPath}");
		return File.ReadAllText(filePath);
	}

	[Fact]
	public async Task GetDetails_ShouldReturnExpected()
	{
		var data = GetTestData("Accounts/GetDetails.json");
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

		var details = await accountClient.GetDetails(
			Guid.Parse("af9d4aa3-5520-437b-b3c8-006ae7c908e8"));
		details.Should().Be(expected);
	}

	[Fact]
	public async Task GetTransactions_ShouldReturnExpected()
	{
		var data = GetTestData("Accounts/GetTransactions.json");
		var bookingDate = new DateTime(2023, 11, 8, 10, 05, 00);
		var valueDate = new DateTime(2023, 11, 10, 12, 12, 12);

		var expected = new Transactions
		{
			Booked = new()
			{
				new()
				{
					AdditionalInformation = "Coffee",
					BankTransactionCode = "PMNT",
					BookingDate = LocalDate.FromDateTime(bookingDate),
					BookingDateTime = Instant.FromDateTimeUtc(bookingDate.ToUniversalTime()),
					CreditorName = "Alderaan Coffee",
					CurrencyExchange = new CurrencyExchange()
					{
						ExchangeRate = decimal.Parse("0.00"),
						SourceCurrency = "GBP",
					},
					EntryReference = "2023111101697308-1",
					MerchantCategoryCode = "123",
					StructuredInformation = "Structured Alderaan - Coffee - Alderaan",
					TransactionAmount = new AmountInCurrency
					{
						Currency = "GBP",
						Amount = decimal.Parse("-10.00"),
					},
					TransactionId = "2023111101697308-1",
					UnstructuredInformation = "Alderaan Coffee - Alderaan",
					ValueDate = LocalDate.FromDateTime(valueDate),
					ValueDateTime = Instant.FromDateTimeUtc(valueDate.ToUniversalTime()),
				},
			},
			Pending = new()
			{
				new()
				{
					AdditionalInformation = "Coffee",
					BankTransactionCode = "PMNT",
					BookingDate = LocalDate.FromDateTime(bookingDate),
					BookingDateTime = Instant.FromDateTimeUtc(bookingDate.ToUniversalTime()),
					CreditorName = "Alderaan Coffee",
					CurrencyExchange = new CurrencyExchange()
					{
						ExchangeRate = decimal.Parse("0.00"),
						SourceCurrency = "GBP",
					},
					EntryReference = "2023111101697308-1",
					MerchantCategoryCode = "123",
					StructuredInformation = "Structured Alderaan - Coffee - Alderaan",
					TransactionAmount = new AmountInCurrency
					{
						Currency = "GBP",
						Amount = decimal.Parse("-10.00"),
					},
					TransactionId = "2023111101697308-1",
					UnstructuredInformation = "Alderaan Coffee - Alderaan",
				},
			},
		};

		var accountClient = new AccountClient(
			MockHttpHandler(data),
			new(DateTimeZoneProviders.Tzdb));
		var details = await accountClient.GetTransactions(
			Guid.Parse("af9d4aa3-5520-437b-b3c8-006ae7c908e8"));
		details.Should().BeEquivalentTo(expected);
	}
}
