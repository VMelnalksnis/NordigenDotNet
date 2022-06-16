// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using NodaTime;

#pragma warning disable SA1623

namespace VMelnalksnis.NordigenDotNet.Requisitions;

/// <summary>A request to access a user's account details from a single institution.</summary>
/// <param name="Id">The id of the requisition.</param>
/// <param name="Created">The point in time when the requisition was created.</param>
/// <param name="Redirect">The URI to which the user will be redirected after authorizing access.</param>
/// <param name="Status">The status of the requisition.</param>
/// <param name="InstitutionId">The ID of the institution for which this requisition was made for.</param>
/// <param name="Reference">Client specified reference for this requisition.</param>
/// <param name="Accounts">Accounts retrieved within the scope of this requisition.</param>
/// <param name="Link">URI to initiate the authorization with the institution.</param>
/// <param name="AccountSelection">Whether to enable account selection view for the end user (if the institution supports it).</param>
/// <param name="RedirectImmediate">Whether to enable redirect back to the client after account list is received.</param>
public record Requisition(
	Guid Id,
	Instant Created,
	Uri Redirect,
	RequisitionStatus Status,
	[property: JsonPropertyName("institution_id")] string InstitutionId,
	string Reference,
	List<Guid> Accounts,
	Uri Link,
	[property: JsonPropertyName("account_selection")] bool AccountSelection,
	[property: JsonPropertyName("redirect_immediate")] bool RedirectImmediate)
{
	/// <summary>The raw value of <see cref="Agreement"/>.</summary>
	[JsonPropertyName("agreement")]
	public string? AgreementValue { get; init; }

	/// <summary>The end-user-agreement of this requisition.</summary>
	[JsonIgnore]
	public Guid? Agreement => string.IsNullOrWhiteSpace(AgreementValue) ? null : Guid.Parse(AgreementValue);

	/// <summary>A two-letter country code (ISO 639-1).</summary>
	[JsonPropertyName("user_language")]
	public string? UserLanguage { get; init; }

	/// <summary>Social security number for account ownership verification.</summary>
	public string? Ssn { get; init; }
}
