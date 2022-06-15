// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Agreements;

/// <inheritdoc />
public sealed class AgreementClient : IAgreementClient
{
	private readonly NordigenHttpClient _nordigenHttpClient;

	/// <summary>Initializes a new instance of the <see cref="AgreementClient"/> class.</summary>
	/// <param name="nordigenHttpClient">Http client configured for making requests to the Nordigen API.</param>
	public AgreementClient(NordigenHttpClient nordigenHttpClient)
	{
		_nordigenHttpClient = nordigenHttpClient;
	}

	/// <inheritdoc />
	public IAsyncEnumerable<EndUserAgreement> Get(int pageSize = 100, CancellationToken cancellationToken = default)
	{
		var requestUri = $"{Routes.Agreements.Uri}?limit={pageSize}&offset=0";
		return _nordigenHttpClient.GetAsJsonPaginated<EndUserAgreement>(requestUri, cancellationToken);
	}

	/// <inheritdoc />
	public async Task<EndUserAgreement> Get(Guid id, CancellationToken cancellationToken = default)
	{
		var agreement = await _nordigenHttpClient
			.GetAsJson<EndUserAgreement>(Routes.Agreements.IdUri(id), cancellationToken)
			.ConfigureAwait(false);

		return agreement!;
	}

	/// <inheritdoc />
	public async Task<EndUserAgreement> Post(EndUserAgreementCreation agreementCreation)
	{
		var agreement = await _nordigenHttpClient
			.PostAsJson<EndUserAgreementCreation, EndUserAgreement>(Routes.Agreements.Uri, agreementCreation)
			.ConfigureAwait(false);

		return agreement!;
	}

	/// <inheritdoc />
	public async Task Delete(Guid id)
	{
		await _nordigenHttpClient.Delete(Routes.Agreements.IdUri(id)).ConfigureAwait(false);
	}

	/// <inheritdoc />
	public async Task<EndUserAgreement> Put(Guid id, EndUserAgreementAcceptance acceptance)
	{
		var agreement = await _nordigenHttpClient
			.PutAsJson<EndUserAgreementAcceptance, EndUserAgreement>(Routes.Agreements.AcceptUri(id), acceptance)
			.ConfigureAwait(false);

		return agreement!;
	}
}
