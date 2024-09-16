using System;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit.Abstractions;

#if NET6_0_OR_GREATER
using System.Net;
#endif

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

		var expectedInstitution = institutions
			.Should()
			.ContainSingle(institution => institution.Id == "CITADELE_PARXLV22")
			.Subject;

		var institution = await _nordigenClient.Institutions.Get(expectedInstitution.Id);
		institution.Should().BeEquivalentTo(expectedInstitution);

		using (new AssertionScope())
		{
			institution.Id.Should().Be("CITADELE_PARXLV22");
			institution.Name.Should().Be("Citadele");
			institution.Bic.Should().Be("PARXLV22");
			institution.TransactionTotalDays.Should().Be(730);
			institution.Countries.Should().ContainSingle().Which.Should().Be("LV");
			institution.Logo.Should().Be(new Uri("https://storage.googleapis.com/gc-prd-institution_icons-production/LV/PNG/citadele.png"));
		}
	}

	[Fact]
	public async Task GetByCountry_ShouldThrowOnInvalidCountry()
	{
		const string countryCode = "FOO";

		var exceptionAssertions = await FluentActions
			.Awaiting(() => _nordigenClient.Institutions.GetByCountry(countryCode))
			.Should()
			.ThrowExactlyAsync<HttpRequestException>()
			.WithMessage($$"""
{"country":{"summary":"Invalid country choice.","detail":"{{countryCode}} is not a valid choice."},"status_code":400}
""");

#if NET6_0_OR_GREATER
		exceptionAssertions.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
#else
		exceptionAssertions.Which.Message.Should().NotBeNullOrWhiteSpace();
#endif
	}
}
