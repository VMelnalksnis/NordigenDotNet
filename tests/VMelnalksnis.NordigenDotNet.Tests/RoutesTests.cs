// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Tests;

public sealed class RoutesTests
{
	[Theory]
	[MemberData(nameof(TestData))]
	public void TransactionsUri_ShouldHaveExpectedQueryParameters(
		Guid id,
		Interval? interval,
		NameValueCollection expectedParameters)
	{
		var uriValue = Routes.Accounts.TransactionsUri(id, interval);
		var uriBuilder = new UriBuilder(uriValue);

		var queryDictionary = HttpUtility.ParseQueryString(uriBuilder.Query);
		queryDictionary.Should().BeEquivalentTo(expectedParameters);
	}

	internal static IEnumerable<object?[]> TestData()
	{
		yield return new object?[] { Guid.NewGuid(), null, new NameValueCollection() };

		var dateFrom = Instant.FromUtc(2022, 05, 01, 12, 00);
		var dateTo = Instant.FromUtc(2022, 05, 12, 13, 00);
		var collection = new NameValueCollection
		{
			{ "date_from", "2022-05-01" },
			{ "date_to", "2022-05-12" },
		};

		yield return new object?[] { Guid.NewGuid(), new Interval(dateFrom, dateTo), collection };
	}
}
