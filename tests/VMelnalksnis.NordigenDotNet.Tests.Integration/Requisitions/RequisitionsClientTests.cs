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

using VMelnalksnis.NordigenDotNet.Requisitions;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Requisitions;

public sealed class RequisitionsClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly INordigenClient _nordigenClient;

	public RequisitionsClientTests(ServiceProviderFixture serviceProviderFixture)
	{
		_nordigenClient = serviceProviderFixture.NordigenClient;
	}

	[Fact]
	public async Task Get()
	{
		var requisitions = await _nordigenClient.Requisitions.Get().ToListAsync();
		var expectedRequisition = requisitions.Should().ContainSingle().Subject;

		var requisition = await _nordigenClient.Requisitions.Get(expectedRequisition.Id);
		requisition.Should().BeEquivalentTo(expectedRequisition);
	}

	[Fact]
	public async Task CreateAndDelete()
	{
		var creation = new RequisitionCreation(new("https://github.com/VMelnalksnis/NordigenDotNet"), "CITADELE_PARXLV22");
		var requisition = await _nordigenClient.Requisitions.Post(creation);
		var requisitions = await _nordigenClient.Requisitions.Get().ToListAsync();

		using (new AssertionScope())
		{
			requisition.Created.Should().BeGreaterThan(SystemClock.Instance.GetCurrentInstant() - Duration.FromSeconds(5));
			requisition.Redirect.Should().Be(creation.Redirect);
			requisition.Status.Should().Be(RequisitionStatus.Cr);
			requisition.InstitutionId.Should().BeEquivalentTo(creation.InstitutionId);
			requisition.Reference.Should().NotBeNullOrWhiteSpace("Nordigen sets it to a random GUID if not specified");
			requisition.Accounts.Should().BeEmpty("accounts are not returned before user authorizes it");
			requisition.Link.AbsoluteUri.Should().Contain("nordigen");
			requisition.AccountSelection.Should().BeFalse();
			requisition.RedirectImmediate.Should().BeFalse();
			requisition.Agreement.Should().BeNull();
			requisition.UserLanguage.Should().BeNull();
			requisition.Ssn.Should().BeNull();
		}

		requisitions
			.Should()
			.ContainSingle(r => r.Id == requisition.Id)
			.Which.Should()
			.BeEquivalentTo(requisition);

		await _nordigenClient.Requisitions.Delete(requisition.Id);

		var getException = await FluentActions
			.Awaiting(() => _nordigenClient.Requisitions.Get(requisition.Id))
			.Should()
			.ThrowExactlyAsync<HttpRequestException>();
#if NET6_0_OR_GREATER
		getException.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
#endif
	}
}
