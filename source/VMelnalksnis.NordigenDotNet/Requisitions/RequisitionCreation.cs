// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Text.Json.Serialization;

using VMelnalksnis.NordigenDotNet.Institutions;

#pragma warning disable SA1623

namespace VMelnalksnis.NordigenDotNet.Requisitions;

/// <summary>Information needed to create a new <see cref="Requisition"/>.</summary>
/// <param name="Redirect">Redirect URL to your application after end-user authorization with ASPSP.</param>
/// <param name="InstitutionId">The id of the <see cref="Institution"/> for which to create a requisition for.</param>
public record RequisitionCreation(Uri Redirect, [property: JsonPropertyName("institution_id")] string InstitutionId)
{
	/// <summary>The end-user-agreement of this requisition.</summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Guid? Agreement { get; init; }

	/// <summary>A unique ID for internal referencing.</summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Reference { get; init; }

	/// <summary>
	/// The language for all end users steps hosted by Nordigen in ISO 639-1 format.
	/// If not defined, the language set in the browser will be used instead.
	/// </summary>
	/// <seealso href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes"/>
	[JsonPropertyName("user_language")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? UserLanguage { get; init; }

	/// <summary>Optional SSN field to verify ownership of the account.</summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Ssn { get; init; }

	/// <summary>Option to enable account selection view for the end user.</summary>
	[JsonPropertyName("account_selection")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public bool? AccountSelection { get; init; }

	/// <summary>Enable redirect back to the client after account list received.</summary>
	[JsonPropertyName("redirect_immediate")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public bool? RedirectImmediate { get; init; }
}
