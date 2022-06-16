// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;

using NodaTime.Testing;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Tokens;

public sealed class TokenDelegatingHandlerTests : IClassFixture<ServiceProviderFixture>
{
	private readonly FakeClock _fakeClock;
	private readonly INordigenClient _nordigenClient;

	public TokenDelegatingHandlerTests(ServiceProviderFixture serviceProviderFixture)
	{
		_fakeClock = serviceProviderFixture.Clock;
		_nordigenClient = serviceProviderFixture.NordigenClient;
	}

	[Theory]
	[InlineData(86400)]
	[InlineData(2592000)]
	public async Task ShouldSucceedAfterPeriod(int seconds)
	{
		var expectedAgreement = await _nordigenClient.Agreements.Get().OrderBy(a => a.Created).FirstAsync();

		_fakeClock.AdvanceSeconds(seconds);

		var agreement = await _nordigenClient.Agreements.Get().OrderBy(a => a.Created).FirstAsync();
		agreement.Should().BeEquivalentTo(expectedAgreement);
	}
}
