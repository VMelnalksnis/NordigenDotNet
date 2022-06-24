// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Agreements;

using Xunit.Abstractions;

using static VMelnalksnis.NordigenDotNet.Tests.Integration.ServiceProviderFixture;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Agreements;

public sealed class AgreementClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly INordigenClient _nordigenClient;

	public AgreementClientTests(ITestOutputHelper testOutputHelper, ServiceProviderFixture serviceProviderFixture)
	{
		_nordigenClient = serviceProviderFixture.GetNordigenClient(testOutputHelper);
	}

	[Fact]
	public async Task Get()
	{
		var creation = new EndUserAgreementCreation(IntegrationInstitutionId);

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
		(await FluentActions
				.Awaiting(() => _nordigenClient.Agreements.Put(createdAgreement.Id, acceptance))
				.Should()
				.ThrowExactlyAsync<HttpRequestException>())
			.Which.StatusCode.Should()
			.Be(HttpStatusCode.Forbidden, "test company cannot create agreements");

		await _nordigenClient.Agreements.Delete(createdAgreement.Id);

		(await FluentActions
				.Awaiting(() => _nordigenClient.Agreements.Get(createdAgreement.Id))
				.Should()
				.ThrowExactlyAsync<HttpRequestException>())
			.Which.StatusCode.Should()
			.Be(HttpStatusCode.NotFound);
	}
}
