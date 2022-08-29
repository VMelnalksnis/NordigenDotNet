// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Serialization;

internal static class HttpClientExtensions
{
	internal static async IAsyncEnumerable<TResult> GetPaginated<TResult>(
		this HttpClient httpClient,
		string requestUri,
		JsonTypeInfo<PaginatedList<TResult>> typeInfo,
		[EnumeratorCancellation] CancellationToken cancellationToken)
		where TResult : class
	{
		var next = requestUri;
		while (next is not null && !cancellationToken.IsCancellationRequested)
		{
			var paginatedList = await httpClient.GetFromJsonAsync(next, typeInfo, cancellationToken).ConfigureAwait(false);
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

	internal static async Task<TResult?> Post<TRequest, TResult>(
		this HttpClient httpClient,
		string requestUri,
		TRequest request,
		JsonTypeInfo<TRequest> requestTypeInfo,
		JsonTypeInfo<TResult> resultTypeInfo)
	{
		var response = await httpClient.PostAsJsonAsync(requestUri, request, requestTypeInfo).ConfigureAwait(false);
		await response.ThrowIfNotSuccessful().ConfigureAwait(false);
		return await response.Content.ReadFromJsonAsync(resultTypeInfo).ConfigureAwait(false);
	}

	internal static async Task<TResult?> Put<TRequest, TResult>(
		this HttpClient httpClient,
		string requestUri,
		TRequest request,
		JsonTypeInfo<TRequest> requestTypeInfo,
		JsonTypeInfo<TResult> resultTypeInfo)
	{
		var response = await httpClient.PutAsJsonAsync(requestUri, request, requestTypeInfo).ConfigureAwait(false);
		await response.ThrowIfNotSuccessful().ConfigureAwait(false);
		return await response.Content.ReadFromJsonAsync(resultTypeInfo).ConfigureAwait(false);
	}

	internal static async Task Delete(this HttpClient httpClient, string requestUri)
	{
		var response = await httpClient.DeleteAsync(requestUri).ConfigureAwait(false);
		await response.ThrowIfNotSuccessful().ConfigureAwait(false);
	}
}
