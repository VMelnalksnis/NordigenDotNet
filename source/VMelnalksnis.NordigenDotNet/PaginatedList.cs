// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace VMelnalksnis.NordigenDotNet;

internal sealed record PaginatedList<TResult>(int? Count, Uri? Next, Uri? Previous, List<TResult>? Results)
	where TResult : class;
