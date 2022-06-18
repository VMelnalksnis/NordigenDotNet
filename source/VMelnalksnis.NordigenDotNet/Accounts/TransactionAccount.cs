// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>An account that can be referenced from a <see cref="Transaction"/>.</summary>
/// <param name="Iban">The IBAN of the account.</param>
public record TransactionAccount(string Iban);
