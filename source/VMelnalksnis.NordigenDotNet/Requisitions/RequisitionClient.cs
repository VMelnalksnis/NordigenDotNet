// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Requisitions;

/// <inheritdoc />
public sealed class RequisitionClient : IRequisitionClient
{
	private readonly NordigenHttpClient _nordigenHttpClient;

	/// <summary>Initializes a new instance of the <see cref="RequisitionClient"/> class.</summary>
	/// <param name="nordigenHttpClient">Http client configured for making requests to the Nordigen API.</param>
	public RequisitionClient(NordigenHttpClient nordigenHttpClient)
	{
		_nordigenHttpClient = nordigenHttpClient;
	}

	/// <inheritdoc />
	public IAsyncEnumerable<Requisition> Get(int pageSize = 100, CancellationToken cancellationToken = default)
	{
		var requestUri = $"{Routes.Requisitions.Uri}?limit={pageSize}&offset=0";
		return _nordigenHttpClient.GetAsJsonPaginated<Requisition>(requestUri, cancellationToken);
	}

	/// <inheritdoc />
	public async Task<Requisition> Get(Guid id, CancellationToken cancellationToken = default)
	{
		var requisition = await _nordigenHttpClient
			.GetAsJson<Requisition>(Routes.Requisitions.IdUri(id), cancellationToken)
			.ConfigureAwait(false);

		return requisition!;
	}

	/// <inheritdoc />
	public async Task<Requisition> Post(RequisitionCreation requisitionCreation)
	{
		var requisition = await _nordigenHttpClient
			.PostAsJson<RequisitionCreation, Requisition>(Routes.Requisitions.Uri, requisitionCreation)
			.ConfigureAwait(false);

		return requisition!;
	}

	/// <inheritdoc />
	public async Task Delete(Guid id)
	{
		await _nordigenHttpClient.Delete(Routes.Requisitions.IdUri(id)).ConfigureAwait(false);
	}
}
