using System;
using System.Text.Json.Serialization;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Institutions;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>Information about the account record, such as the processing status and IBAN.</summary>
public record Account
{
	/// <summary>Gets or sets the id of this Account, used to refer to this account in other API calls.</summary>
	public Guid Id { get; set; }

	/// <summary>Gets or sets the point in time when the account object was created.</summary>
	public Instant Created { get; set; }

	/// <summary>Gets or sets the point in time when the account object was last accessed.</summary>
	[JsonPropertyName("last_accessed")]
	public Instant? LastAccessed { get; set; }

	/// <summary>Gets or sets the account IBAN.</summary>
	public string Iban { get; set; } = null!;

	/// <summary>Gets or sets the id of the <see cref="Institution"/> associated with the account.</summary>
	[JsonPropertyName("institution_id")]
	public string InstitutionId { get; set; } = null!;

	/// <summary>Gets or sets the processing status of this account.</summary>
	public AccountStatus Status { get; set; }
}
