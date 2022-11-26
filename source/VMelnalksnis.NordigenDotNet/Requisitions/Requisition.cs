// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Requisitions;

/// <summary>A request to access a user's account details from a single institution.</summary>
public record Requisition
{
	/// <summary>Gets or sets the id of the requisition.</summary>
	public Guid Id { get; set; }

	/// <summary>Gets or sets the point in time when the requisition was created.</summary>
	public Instant Created { get; set; }

	/// <summary>Gets or sets the URI to which the user will be redirected after authorizing access.</summary>
	public Uri Redirect { get; set; } = null!;

	/// <summary>Gets or sets the status of the requisition.</summary>
	public RequisitionStatus Status { get; set; }

	/// <summary>Gets or sets the ID of the institution for which this requisition was made for.</summary>
	[JsonPropertyName("institution_id")]
	public string InstitutionId { get; set; } = null!;

	/// <summary>Gets or sets client specified reference for this requisition.</summary>
	public string Reference { get; set; } = null!;

	/// <summary>Gets or sets accounts retrieved within the scope of this requisition.</summary>
	public List<Guid> Accounts { get; set; } = null!;

	/// <summary>Gets or sets uRI to initiate the authorization with the institution.</summary>
	public Uri Link { get; set; } = null!;

	/// <summary>Gets or sets a value indicating whether whether to enable account selection view for the end user (if the institution supports it).</summary>
	[JsonPropertyName("account_selection")]
	public bool AccountSelection { get; set; }

	/// <summary>Gets or sets a value indicating whether whether to enable redirect back to the client after account list is received.</summary>
	[JsonPropertyName("redirect_immediate")]
	public bool RedirectImmediate { get; set; }

	/// <summary>Gets or sets the raw value of <see cref="Agreement"/>.</summary>
	[JsonPropertyName("agreement")]
	public string? AgreementValue { get; set; }

	/// <summary>Gets the end-user-agreement of this requisition.</summary>
	[JsonIgnore]
	public Guid? Agreement => string.IsNullOrWhiteSpace(AgreementValue) ? null : Guid.Parse(AgreementValue!);

	/// <summary>Gets or sets a two-letter country code (ISO 639-1).</summary>
	[JsonPropertyName("user_language")]
	public string? UserLanguage { get; set; }

	/// <summary>Gets or sets social security number for account ownership verification.</summary>
	public string? Ssn { get; set; }
}
