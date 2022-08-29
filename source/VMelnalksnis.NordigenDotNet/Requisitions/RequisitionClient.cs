// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Requisitions;

/// <inheritdoc />
public sealed class RequisitionClient : IRequisitionClient
{
	private readonly HttpClient _httpClient;
	private readonly NordigenSerializationContext _context;

	/// <summary>Initializes a new instance of the <see cref="RequisitionClient"/> class.</summary>
	/// <param name="httpClient">Http client configured for making requests to the Nordigen API.</param>
	/// <param name="serializerOptions">Nordigen specific instance of <see cref="JsonSerializerOptions"/>.</param>
	public RequisitionClient(HttpClient httpClient, NordigenJsonSerializerOptions serializerOptions)
	{
		_httpClient = httpClient;
		_context = serializerOptions.Context;
	}

	/// <inheritdoc />
	public IAsyncEnumerable<Requisition> Get(int pageSize = 100, CancellationToken cancellationToken = default)
	{
		return _httpClient.GetPaginated(
			Routes.Requisitions.PaginatedUri(pageSize),
			_context.PaginatedListRequisition,
			cancellationToken);
	}

	/// <inheritdoc />
	public Task<Requisition> Get(Guid id, CancellationToken cancellationToken = default)
	{
		return _httpClient.GetFromJsonAsync(
			Routes.Requisitions.IdUri(id),
			_context.Requisition,
			cancellationToken)!;
	}

	/// <inheritdoc />
	public Task<Requisition> Post(RequisitionCreation requisitionCreation)
	{
		return _httpClient.Post(
			Routes.Requisitions.Uri,
			requisitionCreation,
			_context.RequisitionCreation,
			_context.Requisition)!;
	}

	/// <inheritdoc />
	public async Task Delete(Guid id)
	{
		await _httpClient.Delete(Routes.Requisitions.IdUri(id)).ConfigureAwait(false);
	}
}
