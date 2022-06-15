// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace VMelnalksnis.NordigenDotNet.Agreements;

/// <summary>Details needed to accept an end-user agreement.</summary>
/// <param name="UserAgent">User agent string for the end user.</param>
/// <param name="IpAddress">End user IP address.</param>
public record EndUserAgreementAcceptance(
	[property: JsonPropertyName("user_agent")] string UserAgent,
	[property: JsonPropertyName("ip_address")] string IpAddress);
