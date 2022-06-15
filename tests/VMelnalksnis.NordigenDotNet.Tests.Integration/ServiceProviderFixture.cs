// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Accounts;
using VMelnalksnis.NordigenDotNet.Agreements;
using VMelnalksnis.NordigenDotNet.DependencyInjection;
using VMelnalksnis.NordigenDotNet.Institutions;
using VMelnalksnis.NordigenDotNet.Requisitions;

namespace VMelnalksnis.NordigenDotNet.Tests.Integration;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public sealed class ServiceProviderFixture
{
	private readonly IServiceProvider _serviceProvider;

	public ServiceProviderFixture()
	{
		var configuration = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddUserSecrets<ServiceProviderFixture>()
			.Build();

		var serviceCollection = new ServiceCollection();
		serviceCollection.AddNordigenDotNet(configuration, DateTimeZoneProviders.Tzdb);

		_serviceProvider = serviceCollection.BuildServiceProvider();
	}

	public IAccountClient AccountClient => _serviceProvider.GetRequiredService<IAccountClient>();

	public IAgreementClient AgreementClient => _serviceProvider.GetRequiredService<IAgreementClient>();

	public IInstitutionClient InstitutionClient => _serviceProvider.GetRequiredService<IInstitutionClient>();

	public IRequisitionClient RequisitionClient => _serviceProvider.GetRequiredService<IRequisitionClient>();
}
