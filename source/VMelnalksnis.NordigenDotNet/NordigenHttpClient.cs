// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace VMelnalksnis.NordigenDotNet;

/// <summary><see cref="HttpClient"/> wrapper that uses the correct serialization settings for all requests.</summary>
public sealed class NordigenHttpClient
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _serializerOptions;

	/// <summary>Initializes a new instance of the <see cref="NordigenHttpClient"/> class.</summary>
	/// <param name="httpClient">Http client configured for making requests to the Nordigen API.</param>
	/// <param name="dateTimeZoneProvider">Time zone provider for date and time serialization.</param>
	public NordigenHttpClient(HttpClient httpClient, IDateTimeZoneProvider dateTimeZoneProvider)
	{
		_httpClient = httpClient;

		_serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
		{
			Converters = { new JsonStringEnumConverter() },
		}.ConfigureForNodaTime(dateTimeZoneProvider);
	}

	internal async Task<TResult?> Get<TResult>(string requestUri, CancellationToken cancellationToken)
	{
		return await _httpClient.GetFromJsonAsync<TResult>(requestUri, _serializerOptions, cancellationToken).ConfigureAwait(false);
	}

	internal async IAsyncEnumerable<TResult> GetPaginated<TResult>(
		string requestUri,
		[EnumeratorCancellation] CancellationToken cancellationToken)
		where TResult : class
	{
		var next = requestUri;
		while (next is not null && !cancellationToken.IsCancellationRequested)
		{
			var paginatedList = await Get<PaginatedList<TResult>>(next, cancellationToken).ConfigureAwait(false);
			if (paginatedList?.Results is null)
			{
				yield break;
			}

			foreach (var result in paginatedList.Results)
			{
				yield return result;
			}

			next = paginatedList.Next?.PathAndQuery;
		}
	}

	internal async Task<TResult?> Post<TRequest, TResult>(string requestUri, TRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync(requestUri, request, _serializerOptions).ConfigureAwait(false);
		await response.ThrowIfNotSuccessful().ConfigureAwait(false);
		return await response.Content.ReadFromJsonAsync<TResult>(_serializerOptions).ConfigureAwait(false);
	}

	internal async Task<TResult?> Put<TRequest, TResult>(string requestUri, TRequest request)
	{
		var response = await _httpClient.PutAsJsonAsync(requestUri, request, _serializerOptions).ConfigureAwait(false);
		await response.ThrowIfNotSuccessful().ConfigureAwait(false);
		return await response.Content.ReadFromJsonAsync<TResult>(_serializerOptions).ConfigureAwait(false);
	}

	internal async Task Delete(string requestUri)
	{
		var response = await _httpClient.DeleteAsync(requestUri).ConfigureAwait(false);
		await response.ThrowIfNotSuccessful().ConfigureAwait(false);
	}
}
