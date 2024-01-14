using System;
using System.Collections.Generic;

namespace VMelnalksnis.NordigenDotNet.Serialization;

internal sealed class PaginatedList<TResult>
	where TResult : class
{
	public int? Count { get; set; }

	public Uri? Next { get; set; }

	public Uri? Previous { get; set; }

	public List<TResult>? Results { get; set; }
}
