using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

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
			var paginatedList = await httpClient.Get(next, typeInfo, cancellationToken).ConfigureAwait(false);
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

	internal static async Task<TResult?> Get<TResult>(
		this HttpClient httpClient,
		[UriString("GET")] string requestUri,
		JsonTypeInfo<TResult> typeInfo,
		CancellationToken cancellationToken)
	{
		using var response = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
		await response.ThrowIfNotSuccessful().ConfigureAwait(false);
		return await response.Content.ReadFromJsonAsync(typeInfo, cancellationToken).ConfigureAwait(false);
	}

	internal static async Task<TResult?> Post<TRequest, TResult>(
		this HttpClient httpClient,
		[UriString("POST")] string requestUri,
		TRequest request,
		JsonTypeInfo<TRequest> requestTypeInfo,
		JsonTypeInfo<TResult> resultTypeInfo)
	{
		using var response = await httpClient.PostAsJsonAsync(requestUri, request, requestTypeInfo).ConfigureAwait(false);
		await response.ThrowIfNotSuccessful().ConfigureAwait(false);
		return await response.Content.ReadFromJsonAsync(resultTypeInfo).ConfigureAwait(false);
	}

	internal static async Task<TResult?> Put<TRequest, TResult>(
		this HttpClient httpClient,
		[UriString("PUT")] string requestUri,
		TRequest request,
		JsonTypeInfo<TRequest> requestTypeInfo,
		JsonTypeInfo<TResult> resultTypeInfo)
	{
		using var response = await httpClient.PutAsJsonAsync(requestUri, request, requestTypeInfo).ConfigureAwait(false);
		await response.ThrowIfNotSuccessful().ConfigureAwait(false);
		return await response.Content.ReadFromJsonAsync(resultTypeInfo).ConfigureAwait(false);
	}

	internal static async Task Delete(this HttpClient httpClient, [UriString("DELETE")] string requestUri)
	{
		using var response = await httpClient.DeleteAsync(requestUri).ConfigureAwait(false);
		await response.ThrowIfNotSuccessful().ConfigureAwait(false);
	}
}
