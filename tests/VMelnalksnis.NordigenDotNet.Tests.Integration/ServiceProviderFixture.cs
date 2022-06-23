// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NodaTime;
using NodaTime.Testing;

using VMelnalksnis.NordigenDotNet.DependencyInjection;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public sealed class ServiceProviderFixture
{
	internal const string IntegrationInstitutionId = "SANDBOXFINANCE_SFIN0000";

	private readonly IServiceProvider _serviceProvider;

	public ServiceProviderFixture()
	{
		Clock = new(SystemClock.Instance.GetCurrentInstant());

		var configuration = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddUserSecrets<ServiceProviderFixture>()
			.Build();

		var serviceCollection = new ServiceCollection();
		serviceCollection.AddNordigenDotNet(configuration, Clock, DateTimeZoneProviders.Tzdb);

		_serviceProvider = serviceCollection.BuildServiceProvider();
	}

	public FakeClock Clock { get; }

	public INordigenClient NordigenClient => _serviceProvider.GetRequiredService<INordigenClient>();
}
