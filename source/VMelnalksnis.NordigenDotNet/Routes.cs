// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet;

internal static class Routes
{
	internal static class Agreements
	{
		internal const string Uri = "api/v2/agreements/enduser/";

		internal static string IdUri(Guid id) => $"{Uri}{id:N}/";

		internal static string AcceptUri(Guid id) => $"{IdUri(id)}accept/";
	}

	internal static class Accounts
	{
		private const string _uri = "api/v2/accounts/";

		internal static string IdUri(Guid id) => $"{_uri}{id:N}/";

		internal static string BalancesUri(Guid id) => $"{IdUri(id)}balances/";

		internal static string DetailsUri(Guid id) => $"{IdUri(id)}details/";

		internal static string TransactionsUri(Guid id, Interval? interval) => interval is null
			? TransactionsUri(id)
			: $"{TransactionsUri(id)}?date_from={interval.Value.Start:yyyy-MM-dd}&date_to={interval.Value.End:yyyy-MM-dd}";

		private static string TransactionsUri(Guid id) => $"{IdUri(id)}transactions/";
	}

	internal static class Institutions
	{
		private const string _uri = "api/v2/institutions/";

		internal static string CountryUri(string country) => $"{_uri}?country={country}";

		internal static string IdUri(string id) => $"{_uri}{id}/";
	}

	internal static class Requisitions
	{
		internal const string Uri = "api/v2/requisitions/";

		internal static string IdUri(Guid id) => $"{Uri}{id:N}/";
	}

	internal static class Tokens
	{
		internal const string New = $"{_uri}new/";
		internal const string Refresh = $"{_uri}refresh/";
		private const string _uri = "api/v2/token/";
	}
}
