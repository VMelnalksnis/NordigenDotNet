using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Requisitions;

using Xunit.Abstractions;

#if NET6_0_OR_GREATER
using System.Net;
#endif

using static VMelnalksnis.NordigenDotNet.Tests.Integration.ServiceProviderFixture;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Requisitions;

public sealed class RequisitionsClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly INordigenClient _nordigenClient;

	public RequisitionsClientTests(ITestOutputHelper testOutputHelper, ServiceProviderFixture serviceProviderFixture)
	{
		_nordigenClient = serviceProviderFixture.GetNordigenClient(testOutputHelper);
	}

	[Fact]
	public async Task Get_ShouldPaginateCorrectly()
	{
		var requisitions = await _nordigenClient.Requisitions.Get().ToListAsync();
		var pageSize = Math.Clamp(requisitions.Count / 5, 1, int.MaxValue);

		var pagedRequisitions = await _nordigenClient.Requisitions.Get(pageSize).ToListAsync();

		pagedRequisitions.Should().HaveCountGreaterThan(0).And.BeEquivalentTo(requisitions);
	}

	[Fact]
	public async Task Get_ShouldRespectCancellationToken()
	{
		var source = new CancellationTokenSource();
		var requisitions = new List<Requisition>();

		await foreach (var requisition in _nordigenClient.Requisitions.Get(1, source.Token))
		{
			requisitions.Add(requisition);
			source.Cancel();
		}

		requisitions.Should().ContainSingle();
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
			requisition.Link.AbsoluteUri.Should().StartWith("https://bankaccountdata.gocardless.com/psd2/start/");
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
#if NET6_0_OR_GREATER
			.Which.StatusCode.Should()
			.Be(HttpStatusCode.NotFound);
#else
			.Which.Should().NotBeNull("test company cannot create agreements");
#endif
	}
}
