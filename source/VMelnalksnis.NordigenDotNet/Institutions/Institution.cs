// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Institutions;

/// <summary>Account servicing payment service provider details.</summary>
/// <param name="Id">The id of the service provider.</param>
/// <param name="Name">The name of the service provider.</param>
/// <param name="Bic">The BIC of the service provider.</param>
/// <param name="TransactionTotalDays">The maximum number of days in the past the service provider will return transactions for.</param>
/// <param name="Countries">All the countries supported by the service provider.</param>
/// <param name="Logo">URI to the service provider's logo.</param>
public record Institution(
	string Id,
	string Name,
	string Bic,
	[property: JsonPropertyName("transaction_total_days")] int TransactionTotalDays,
	List<string> Countries,
	Uri Logo);
