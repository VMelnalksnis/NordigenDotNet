// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using VMelnalksnis.NordigenDotNet.Agreements;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Agreements;

public sealed class AgreementClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly IAgreementClient _agreementClient;

	public AgreementClientTests(ServiceProviderFixture serviceProviderFixture)
	{
		_agreementClient = serviceProviderFixture.AgreementClient;
	}

	[Fact]
	public async Task Get()
	{
		var creation = new EndUserAgreementCreation("CITADELE_PARXLV22");

		var createdAgreement = await _agreementClient.Post(creation);
		var agreements = await _agreementClient.Get().ToListAsync();

		agreements
			.Should()
			.ContainSingle(agreement => agreement.Id == createdAgreement.Id)
			.Which.Should()
			.BeEquivalentTo(createdAgreement);

		var acceptance = new EndUserAgreementAcceptance("NordigenDotNet integration test", "127.0.0.1");
		(await FluentActions
				.Awaiting(() => _agreementClient.Put(createdAgreement.Id, acceptance))
				.Should()
				.ThrowExactlyAsync<HttpRequestException>())
			.Which.StatusCode.Should()
			.Be(HttpStatusCode.Forbidden, "test company cannot create agreements");

		await _agreementClient.Delete(createdAgreement.Id);

		(await FluentActions
				.Awaiting(() => _agreementClient.Get(createdAgreement.Id))
				.Should()
				.ThrowExactlyAsync<HttpRequestException>())
			.Which.StatusCode.Should()
			.Be(HttpStatusCode.NotFound);
	}
}
