// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NodaTime;
using NodaTime.Testing;

using Serilog;

using VMelnalksnis.NordigenDotNet.DependencyInjection;

using Xunit.Abstractions;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace VMelnalksnis.NordigenDotNet.Tests.Integration;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public sealed class ServiceProviderFixture
{
	internal const string IntegrationInstitutionId = "SANDBOXFINANCE_SFIN0000";

	public ServiceProviderFixture()
	{
		Clock = new(SystemClock.Instance.GetCurrentInstant());
	}

	public FakeClock Clock { get; }

	public INordigenClient GetNordigenClient(ITestOutputHelper testOutputHelper)
	{
		var configuration = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddUserSecrets<ServiceProviderFixture>()
			.Build();

		var serviceCollection = new ServiceCollection();
		serviceCollection.AddNordigenDotNet(configuration, Clock, DateTimeZoneProviders.Tzdb);
		serviceCollection.AddLogging(builder =>
		{
			var logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.TestOutput(
					testOutputHelper,
					outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
				.Enrich.FromLogContext()
				.CreateLogger();

			builder.AddSerilog(logger);
		});

		return serviceCollection.BuildServiceProvider().GetRequiredService<INordigenClient>();
	}
}
