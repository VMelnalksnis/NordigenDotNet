// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

using VMelnalksnis.NordigenDotNet.Accounts;
using VMelnalksnis.NordigenDotNet.Agreements;
using VMelnalksnis.NordigenDotNet.Institutions;
using VMelnalksnis.NordigenDotNet.Requisitions;

namespace VMelnalksnis.NordigenDotNet;

/// <inheritdoc />
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata, IgnoreReadOnlyProperties = true)]
[JsonSerializable(typeof(Account))]
[JsonSerializable(typeof(AccountDetailsWrapper))]
[JsonSerializable(typeof(BalancesWrapper))]
[JsonSerializable(typeof(TransactionsWrapper))]
[JsonSerializable(typeof(EndUserAgreement))]
[JsonSerializable(typeof(PaginatedList<EndUserAgreement>))]
[JsonSerializable(typeof(EndUserAgreementAcceptance))]
[JsonSerializable(typeof(EndUserAgreementCreation))]
[JsonSerializable(typeof(Institution))]
[JsonSerializable(typeof(List<Institution>))]
[JsonSerializable(typeof(Requisition))]
[JsonSerializable(typeof(PaginatedList<Requisition>))]
[JsonSerializable(typeof(RequisitionCreation))]
internal partial class NordigenSerializationContext : JsonSerializerContext
{
}
