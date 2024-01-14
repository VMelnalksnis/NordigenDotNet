using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using VMelnalksnis.NordigenDotNet.Serialization;

namespace VMelnalksnis.NordigenDotNet.Agreements;

/// <inheritdoc />
public sealed class AgreementClient : IAgreementClient
{
	private readonly HttpClient _httpClient;
	private readonly NordigenSerializationContext _context;

	/// <summary>Initializes a new instance of the <see cref="AgreementClient"/> class.</summary>
	/// <param name="httpClient">Http client configured for making requests to the Nordigen API.</param>
	/// <param name="serializerOptions">Nordigen specific instance of <see cref="JsonSerializerOptions"/>.</param>
	public AgreementClient(HttpClient httpClient, NordigenJsonSerializerOptions serializerOptions)
	{
		_httpClient = httpClient;
		_context = serializerOptions.Context;
	}

	/// <inheritdoc />
	public IAsyncEnumerable<EndUserAgreement> Get(int pageSize = 100, CancellationToken cancellationToken = default)
	{
		return _httpClient.GetPaginated(
			Routes.Agreements.PaginatedUri(pageSize),
			_context.PaginatedListEndUserAgreement,
			cancellationToken);
	}

	/// <inheritdoc />
	public Task<EndUserAgreement> Get(Guid id, CancellationToken cancellationToken = default)
	{
		return _httpClient.Get(Routes.Agreements.IdUri(id), _context.EndUserAgreement, cancellationToken)!;
	}

	/// <inheritdoc />
	public Task<EndUserAgreement> Post(EndUserAgreementCreation agreementCreation)
	{
		return _httpClient.Post(
			Routes.Agreements.Uri,
			agreementCreation,
			_context.EndUserAgreementCreation,
			_context.EndUserAgreement)!;
	}

	/// <inheritdoc />
	public Task Delete(Guid id)
	{
		return _httpClient.Delete(Routes.Agreements.IdUri(id));
	}

	/// <inheritdoc />
	public Task<EndUserAgreement> Put(Guid id, EndUserAgreementAcceptance acceptance)
	{
		return _httpClient.Put(
			Routes.Agreements.AcceptUri(id),
			acceptance,
			_context.EndUserAgreementAcceptance,
			_context.EndUserAgreement)!;
	}
}
