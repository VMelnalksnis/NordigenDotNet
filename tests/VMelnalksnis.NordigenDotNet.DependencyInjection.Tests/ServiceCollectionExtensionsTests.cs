// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NodaTime;

namespace VMelnalksnis.NordigenDotNet.DependencyInjection.Tests;

public sealed class ServiceCollectionExtensionsTests
{
	[Fact]
	public void AddNordigenDotNet_ShouldRegisterRequiredServices()
	{
		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
			{
				new($"{NordigenOptions.SectionName}:{nameof(NordigenOptions.SecretId)}", $"{Guid.NewGuid():D}"),
				new($"{NordigenOptions.SectionName}:{nameof(NordigenOptions.SecretKey)}", $"{Guid.NewGuid():N}"),
			})
			.Build();

		var serviceCollection = new ServiceCollection();

		serviceCollection
			.AddSingleton<IClock>(SystemClock.Instance)
			.AddSingleton(DateTimeZoneProviders.Tzdb)
			.AddNordigenDotNet(configuration);

		var serviceProvider = serviceCollection.BuildServiceProvider();
		var nordigenClient = serviceProvider.GetRequiredService<INordigenClient>();

		nordigenClient.Should().NotBeNull();
	}
}
