// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>An amount in a specific currency.</summary>
/// <param name="Currency">A ISO 4217 currency code.</param>
/// <param name="Amount">The amount.</param>
public record AmountInCurrency(string Currency, decimal Amount);
