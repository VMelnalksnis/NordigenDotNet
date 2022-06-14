// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Requisitions;

/// <summary>Nordigen API client for managing requisitions.</summary>
public interface IRequisitionClient
{
	/// <summary>Gets all requisitions.</summary>
	/// <param name="pageSize">The number of requisitions to get in a single request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages.</returns>
	IAsyncEnumerable<Requisition> Get(int pageSize = 100, CancellationToken cancellationToken = default);

	/// <summary>Gets the requisition with the specified id.</summary>
	/// <param name="id">The id of the requisition to get.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The requisition with the specified id.</returns>
	Task<Requisition> Get(Guid id, CancellationToken cancellationToken = default);

	/// <summary>Creates a new requisition.</summary>
	/// <param name="requisitionCreation">The requisition to create.</param>
	/// <returns>The created requisition.</returns>
	Task<Requisition> Post(RequisitionCreation requisitionCreation);

	/// <summary>Deletes the specified requisition.</summary>
	/// <param name="id">The id of the requisition to delete.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	Task Delete(Guid id);
}
