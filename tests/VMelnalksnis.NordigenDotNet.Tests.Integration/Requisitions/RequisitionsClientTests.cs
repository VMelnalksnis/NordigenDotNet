// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using VMelnalksnis.NordigenDotNet.Requisitions;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Requisitions;

public sealed class RequisitionsClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly IRequisitionClient _requisitionClient;

	public RequisitionsClientTests(ServiceProviderFixture serviceProviderFixture)
	{
		_requisitionClient = serviceProviderFixture.RequisitionClient;
	}

	[Fact]
	public async Task Get()
	{
		var requisitions = await _requisitionClient.Get().ToListAsync();
		var expectedRequisition = requisitions.Should().ContainSingle().Subject;

		var requisition = await _requisitionClient.Get(expectedRequisition.Id);
		requisition.Should().BeEquivalentTo(expectedRequisition);
	}

	[Fact]
	public async Task Post()
	{
		var creation = new RequisitionCreation(new("https://gnomeshade.org/"), "CITADELE_PARXLV22");

		var requisition = await _requisitionClient.Post(creation);

		await _requisitionClient.Delete(requisition.Id);

		(await FluentActions
				.Awaiting(() => _requisitionClient.Get(requisition.Id))
				.Should()
				.ThrowExactlyAsync<HttpRequestException>())
			.Which.StatusCode
			.Should()
			.Be(HttpStatusCode.NotFound);
	}
}
