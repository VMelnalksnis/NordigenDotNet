// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Requisitions;

using static VMelnalksnis.NordigenDotNet.Tests.Integration.ServiceProviderFixture;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Requisitions;

public sealed class RequisitionsClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly INordigenClient _nordigenClient;

	public RequisitionsClientTests(ServiceProviderFixture serviceProviderFixture)
	{
		_nordigenClient = serviceProviderFixture.NordigenClient;
	}

	[Fact]
	public async Task CreateAndDelete()
	{
		var creation = new RequisitionCreation(new("https://github.com/VMelnalksnis/NordigenDotNet"), IntegrationInstitutionId);
		var createdRequisition = await _nordigenClient.Requisitions.Post(creation);
		var requisition = await _nordigenClient.Requisitions.Get(createdRequisition.Id);
		var requisitions = await _nordigenClient.Requisitions.Get().ToListAsync();

		await _nordigenClient.Requisitions.Delete(requisition.Id);

		using (new AssertionScope())
		{
			requisition.Should().BeEquivalentTo(createdRequisition);
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

		(await FluentActions
				.Awaiting(() => _nordigenClient.Requisitions.Get(requisition.Id))
				.Should()
				.ThrowExactlyAsync<HttpRequestException>())
			.Which.StatusCode
			.Should()
			.Be(HttpStatusCode.NotFound);
	}
}
