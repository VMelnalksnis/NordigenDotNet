// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Institutions;

/// <inheritdoc />
public sealed class InstitutionClient : IInstitutionClient
{
	private readonly NordigenHttpClient _nordigenHttpClient;

	/// <summary>Initializes a new instance of the <see cref="InstitutionClient"/> class.</summary>
	/// <param name="nordigenHttpClient">Http client configured for making requests to the Nordigen API.</param>
	public InstitutionClient(NordigenHttpClient nordigenHttpClient)
	{
		_nordigenHttpClient = nordigenHttpClient;
	}

	/// <inheritdoc />
	public async Task<List<Institution>> GetByCountry(string countryCode, CancellationToken cancellationToken = default)
	{
		var institutions = await _nordigenHttpClient
			.Get<List<Institution>>(Routes.Institutions.CountryUri(countryCode), cancellationToken)
			.ConfigureAwait(false);

		return institutions!;
	}

	/// <inheritdoc />
	public async Task<Institution> Get(string id, CancellationToken cancellationToken = default)
	{
		var institution = await _nordigenHttpClient
			.Get<Institution>(Routes.Institutions.IdUri(id), cancellationToken)
			.ConfigureAwait(false);

		return institution!;
	}
}
