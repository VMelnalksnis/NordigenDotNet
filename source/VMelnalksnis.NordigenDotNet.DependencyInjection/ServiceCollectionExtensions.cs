// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using NodaTime;

using VMelnalksnis.NordigenDotNet.Accounts;
using VMelnalksnis.NordigenDotNet.Agreements;
using VMelnalksnis.NordigenDotNet.Institutions;
using VMelnalksnis.NordigenDotNet.Requisitions;

namespace VMelnalksnis.NordigenDotNet.DependencyInjection;

/// <summary>Collection of methods for configuring VMelnalksnis.NordigenDotNet in <see cref="IServiceCollection"/>.</summary>
[PublicAPI]
public static class ServiceCollectionExtensions
{
	/// <summary>Adds all required services for <see cref="INordigenClient"/>.</summary>
	/// <param name="serviceCollection">The service collection in which to register the services.</param>
	/// <param name="configuration">The configuration to which to bind options models.</param>
	/// <param name="dateTimeZoneProvider">Time zone provider for date and time serialization.</param>
	/// <returns>The <see cref="IHttpClientBuilder"/> for the <see cref="HttpClient"/> used by <see cref="INordigenClient"/>.</returns>
	public static IHttpClientBuilder AddNordigenDotNet(
		this IServiceCollection serviceCollection,
		IConfiguration configuration,
		IDateTimeZoneProvider dateTimeZoneProvider)
	{
		serviceCollection.TryAddSingleton(dateTimeZoneProvider);
		return serviceCollection.AddNordigenDotNet(configuration);
	}

	/// <summary>Adds all required services for <see cref="INordigenClient"/>, excluding external dependencies.</summary>
	/// <param name="serviceCollection">The service collection in which to register the services.</param>
	/// <param name="configuration">The configuration to which to bind options models.</param>
	/// <returns>The <see cref="IHttpClientBuilder"/> for the <see cref="HttpClient"/> used by <see cref="INordigenClient"/>.</returns>
	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026", Justification = $"{nameof(NordigenOptions)} contains only system types.")]
	public static IHttpClientBuilder AddNordigenDotNet(
		this IServiceCollection serviceCollection,
		IConfiguration configuration)
	{
		serviceCollection
			.AddOptions<NordigenOptions>()
			.Bind(configuration.GetSection(NordigenOptions.SectionName))
			.ValidateDataAnnotations();

		return serviceCollection
			.AddTransient<INordigenClient, NordigenClient>()
			.AddTransient<IAccountClient, AccountClient>()
			.AddTransient<IAgreementClient, AgreementClient>()
			.AddTransient<IInstitutionClient, InstitutionClient>()
			.AddTransient<IRequisitionClient, RequisitionClient>()
			.AddTransient(provider => provider.GetRequiredService<IOptionsSnapshot<NordigenOptions>>().Value)
			.AddHttpClient<NordigenHttpClient>((provider, client) =>
			{
				client.BaseAddress = provider.GetRequiredService<IOptionsSnapshot<NordigenOptions>>().Value.BaseAddress;

				var assembly = typeof(INordigenClient).Assembly.GetName();

				var assemblyName = assembly.Name ??
					throw new InvalidOperationException($"Assembly {assembly.FullName} name is not specified");

				var assemblyVersion = assembly.Version ??
					throw new InvalidOperationException($"Assembly {assembly.FullName} version is not specified");

				client.DefaultRequestHeaders.UserAgent.Add(new(assemblyName, assemblyVersion.ToString()));
			});
	}
}
