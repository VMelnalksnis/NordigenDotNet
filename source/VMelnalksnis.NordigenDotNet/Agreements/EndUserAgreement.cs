// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Institutions;

#pragma warning disable SA1623

namespace VMelnalksnis.NordigenDotNet.Agreements;

/// <summary>Details the end-user has agreement to share from a specific institution.</summary>
/// <param name="Id">The id of the end-user agreement.</param>
/// <param name="Created">The point in time when the agreement was created.</param>
/// <param name="MaxHistoricalDays">Maximum number of days of transaction data to retrieve.</param>
/// <param name="AccessValidForDays">Number of days from acceptance that the access can be used.</param>
/// <param name="AccessScope">Level of information to access.</param>
/// <param name="InstitutionId">An <see cref="Institution"/> ID for the agreement.</param>
public record EndUserAgreement(
	Guid Id,
	Instant Created,
	[property: JsonPropertyName("max_historical_days")] int MaxHistoricalDays,
	[property: JsonPropertyName("access_valid_for_days")] int AccessValidForDays,
	[property: JsonPropertyName("access_scope")] List<string> AccessScope,
	[property: JsonPropertyName("institution_id")] string InstitutionId)
{
	/// <summary>The point int time when the end user accepted the agreement.</summary>
	public Instant? Accepted { get; init; }
}
