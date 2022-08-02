// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Specialized;
using System.Web;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Tests;

public sealed class RoutesTests
{
	[Theory]
	[ClassData(typeof(TransactionsUriTestData))]
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
}
