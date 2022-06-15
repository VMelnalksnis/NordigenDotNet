// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Agreements;

/// <summary>Nordigen API client for managing agreements.</summary>
public interface IAgreementClient
{
	/// <summary>Gets all agreements.</summary>
	/// <param name="pageSize">The number of agreements to get in a single request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages.</returns>
	IAsyncEnumerable<EndUserAgreement> Get(int pageSize = 100, CancellationToken cancellationToken = default);

	/// <summary>Gets the agreement with the specified id.</summary>
	/// <param name="id">The id of the agreement to get.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The agreement with the specified id.</returns>
	Task<EndUserAgreement> Get(Guid id, CancellationToken cancellationToken = default);

	/// <summary>Creates a new agreement.</summary>
	/// <param name="agreementCreation">The agreement to create.</param>
	/// <returns>The created agreement.</returns>
	Task<EndUserAgreement> Post(EndUserAgreementCreation agreementCreation);

	/// <summary>Deletes the specified agreement.</summary>
	/// <param name="id">The id of the agreement to delete.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	Task Delete(Guid id);

	/// <summary>Accepts the specified agreement.</summary>
	/// <param name="id">The id of the agreement to accept.</param>
	/// <param name="agreementAcceptance">Details from the end-user needed to accept the agreement.</param>
	/// <returns>The accepted agreement.</returns>
	Task<EndUserAgreement> Put(Guid id, EndUserAgreementAcceptance agreementAcceptance);
}
