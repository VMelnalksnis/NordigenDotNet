// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Text.Json.Serialization;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Institutions;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>Information about the account record, such as the processing status and IBAN.</summary>
/// <param name="Id">The ID of this Account, used to refer to this account in other API calls.</param>
/// <param name="Created">The point in time when the account object was created.</param>
/// <param name="LastAccessed">The point in time when the account object was last accessed.</param>
/// <param name="Iban">The account IBAN.</param>
/// <param name="InstitutionId">The id of the <see cref="Institution"/> associated with the account.</param>
/// <param name="Status">The processing status of this account.</param>
public record Account(
	Guid Id,
	Instant Created,
	[property: JsonPropertyName("last_accessed")] Instant LastAccessed,
	string Iban,
	[property: JsonPropertyName("institution_id")] string InstitutionId,
	AccountStatus Status);
