// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

using VMelnalksnis.NordigenDotNet.Institutions;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Institutions;

public sealed class InstitutionClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly IInstitutionClient _institutionClient;

	public InstitutionClientTests(ServiceProviderFixture serviceProviderFixture)
	{
		_institutionClient = serviceProviderFixture.InstitutionClient;
	}

	[Fact]
	public async Task GetByCountry()
	{
		var institutions = await _institutionClient.GetByCountry("LV");

		institutions.Count.Should().Be(17);
		var expectedInstitution = institutions.Should().ContainSingle(i => i.Id == "CITADELE_PARXLV22").Subject;

		var institution = await _institutionClient.Get(expectedInstitution.Id);
		institution.Should().BeEquivalentTo(expectedInstitution);
	}
}
