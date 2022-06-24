// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Institutions;

public sealed class InstitutionClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly INordigenClient _nordigenClient;

	public InstitutionClientTests(ITestOutputHelper testOutputHelper, ServiceProviderFixture serviceProviderFixture)
	{
		_nordigenClient = serviceProviderFixture.GetNordigenClient(testOutputHelper);
	}

	[Fact]
	public async Task GetByCountry()
	{
		var institutions = await _nordigenClient.Institutions.GetByCountry("LV");

		institutions.Count.Should().Be(17);
		var expectedInstitution = institutions.Should().ContainSingle(i => i.Id == "CITADELE_PARXLV22").Subject;

		var institution = await _nordigenClient.Institutions.Get(expectedInstitution.Id);
		institution.Should().BeEquivalentTo(expectedInstitution);

		using (new AssertionScope())
		{
			institution.Id.Should().Be("CITADELE_PARXLV22");
			institution.Name.Should().Be("Citadele");
			institution.Bic.Should().Be("PARXLV22");
			institution.TransactionTotalDays.Should().Be(730);
			institution.Countries.Should().ContainSingle().Which.Should().Be("LV");
			institution.Logo.Should().Be(new Uri("https://cdn.nordigen.com/ais/CITADELE_PARXLV22.png"));
		}
	}
}
