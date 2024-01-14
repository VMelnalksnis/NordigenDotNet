using System;
using System.Text.Json.Serialization;

using VMelnalksnis.NordigenDotNet.Institutions;

namespace VMelnalksnis.NordigenDotNet.Requisitions;

/// <summary>Information needed to create a new <see cref="Requisition"/>.</summary>
public record RequisitionCreation(Uri Redirect, string InstitutionId)
{
	/// <summary>Gets redirect URL to your application after end-user authorization with ASPSP.</summary>
	public Uri Redirect { get; } = Redirect;

	/// <summary>Gets the id of the <see cref="Institution"/> for which to create a requisition for.</summary>
	[JsonPropertyName("institution_id")]
	public string InstitutionId { get; } = InstitutionId;

	/// <summary>Gets or sets the end-user-agreement of this requisition.</summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Guid? Agreement { get; set; }

	/// <summary>Gets or sets a unique ID for internal referencing.</summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Reference { get; set; }

	/// <summary>
	/// Gets or sets the language for all end users steps hosted by Nordigen in ISO 639-1 format.
	/// If not defined, the language set in the browser will be used instead.
	/// </summary>
	/// <seealso href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes"/>
	[JsonPropertyName("user_language")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? UserLanguage { get; set; }

	/// <summary>Gets or sets optional SSN field to verify ownership of the account.</summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Ssn { get; set; }

	/// <summary>Gets or sets option to enable account selection view for the end user.</summary>
	[JsonPropertyName("account_selection")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public bool? AccountSelection { get; set; }

	/// <summary>Gets or sets enable redirect back to the client after account list received.</summary>
	[JsonPropertyName("redirect_immediate")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public bool? RedirectImmediate { get; set; }
}
