using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Tests;

internal sealed class TransactionsUriTestData : IEnumerable<object?[]>
{
	public IEnumerator<object?[]> GetEnumerator()
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

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
