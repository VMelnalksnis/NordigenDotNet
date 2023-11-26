﻿using System;
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
		var expected = new Transactions
		{
			Booked =
			[
				new()
				{
					AdditionalInformation = "Coffee",
					BankTransactionCode = "PMNT",
					BookingDate = bookingDate.InZone(DateTimeZone.Utc).Date,
					BookingDateTime = bookingDate,
					CreditorName = "Alderaan Coffee",
					CurrencyExchange = new()
					{
						ExchangeRate = 0.00m,
						SourceCurrency = "GBP",
					},
					EntryReference = "2023111101697308-1",
					MerchantCategoryCode = "123",
					StructuredInformation = "Structured Alderaan - Coffee - Alderaan",
					TransactionAmount = new()
					{
						Currency = "GBP",
						Amount = -10.00m,
					},
					TransactionId = "2023111101697308-1",
					UnstructuredInformation = "Alderaan Coffee - Alderaan",
					ValueDate = valueDate.InZone(DateTimeZone.Utc).Date,
					ValueDateTime = valueDate,
				},
			],
			Pending =
			[
				new()
				{
					AdditionalInformation = "Coffee",
					BankTransactionCode = "PMNT",
					BookingDate = bookingDate.InZone(DateTimeZone.Utc).Date,
					BookingDateTime = bookingDate,
					CreditorName = "Alderaan Coffee",
					CurrencyExchange = new()
					{
						ExchangeRate = 0.00m,
						SourceCurrency = "GBP",
					},
					EntryReference = "2023111101697308-1",
					MerchantCategoryCode = "123",
					StructuredInformation = "Structured Alderaan - Coffee - Alderaan",
					TransactionAmount = new()
					{
						Currency = "GBP",
						Amount = -10.00m,
					},
					TransactionId = "2023111101697308-1",
					UnstructuredInformation = "Alderaan Coffee - Alderaan",
				},
			],
		};

		var accountClient = new AccountClient(
			MockHttpHandler(data),
			new(DateTimeZoneProviders.Tzdb));

		var transactions = await accountClient.GetTransactions(Guid.NewGuid());

		transactions.Should().BeEquivalentTo(expected);
	}

	private static HttpClient MockHttpHandler(string json) => new(new MockHttpMessageHandler(json))
	{
		BaseAddress = new("http://localhost"),
	};
}
