// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.Accounts;

public record Transaction(AmountInCurrency TransactionAmount, string RemittanceInformationUnstructured)
{
	private LocalDate? ValueDate { get; init; }
}
