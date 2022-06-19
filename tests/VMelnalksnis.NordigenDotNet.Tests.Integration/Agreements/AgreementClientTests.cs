// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;

#if NET6_0_OR_GREATER
using System.Net;
#endif
using System.Net.Http;
using System.Threading.Tasks;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Agreements;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Agreements;

public sealed class AgreementClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly INordigenClient _nordigenClient;

	public AgreementClientTests(ServiceProviderFixture serviceProviderFixture)
	{
		_nordigenClient = serviceProviderFixture.NordigenClient;
	}

	[Fact]
	public async Task Get()
	{
		var creation = new EndUserAgreementCreation("CITADELE_PARXLV22");

		var createdAgreement = await _nordigenClient.Agreements.Post(creation);
		(await _nordigenClient.Agreements.Get(createdAgreement.Id)).Should().BeEquivalentTo(createdAgreement);

		using (new AssertionScope())
		{
			createdAgreement.Created.Should().BeGreaterThan(SystemClock.Instance.GetCurrentInstant() - Duration.FromSeconds(5));
			createdAgreement.Accepted.Should().BeNull();
			createdAgreement.MaxHistoricalDays.Should().Be(90);
			createdAgreement.AccessValidForDays.Should().Be(90);
			createdAgreement.AccessScope.Should().BeEquivalentTo("balances", "details", "transactions");
			createdAgreement.InstitutionId.Should().Be(creation.InstitutionId);
		}

		var agreements = await _nordigenClient.Agreements.Get().ToListAsync();

		agreements
			.Should()
			.ContainSingle(agreement => agreement.Id == createdAgreement.Id)
			.Which.Should()
			.BeEquivalentTo(createdAgreement);

		var acceptance = new EndUserAgreementAcceptance("NordigenDotNet integration test", "127.0.0.1");
		var putException = await FluentActions
				.Awaiting(() => _nordigenClient.Agreements.Put(createdAgreement.Id, acceptance))
				.Should()
				.ThrowExactlyAsync<HttpRequestException>();
#if NET6_0_OR_GREATER
		putException.Which.StatusCode.Should().Be(HttpStatusCode.Forbidden, "test company cannot create agreements");
#endif

		await _nordigenClient.Agreements.Delete(createdAgreement.Id);

		var getException = await FluentActions
				.Awaiting(() => _nordigenClient.Agreements.Get(createdAgreement.Id))
				.Should()
				.ThrowExactlyAsync<HttpRequestException>();
#if NET6_0_OR_GREATER
		getException.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
#endif
	}
}
