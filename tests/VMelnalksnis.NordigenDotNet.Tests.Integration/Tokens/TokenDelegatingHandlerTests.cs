using System.Linq;
using System.Threading.Tasks;

using NodaTime.Testing;

using Xunit.Abstractions;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration.Tokens;

public sealed class TokenDelegatingHandlerTests : IClassFixture<ServiceProviderFixture>
{
	private readonly FakeClock _fakeClock;
	private readonly INordigenClient _nordigenClient;

	public TokenDelegatingHandlerTests(ITestOutputHelper testOutputHelper, ServiceProviderFixture serviceProviderFixture)
	{
		_fakeClock = serviceProviderFixture.Clock;
		_nordigenClient = serviceProviderFixture.GetNordigenClient(testOutputHelper);
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
