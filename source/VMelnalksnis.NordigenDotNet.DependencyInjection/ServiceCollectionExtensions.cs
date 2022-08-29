// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

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
using VMelnalksnis.NordigenDotNet.Serialization;
using VMelnalksnis.NordigenDotNet.Tokens;

namespace VMelnalksnis.NordigenDotNet.DependencyInjection;

/// <summary>Collection of methods for configuring VMelnalksnis.NordigenDotNet in <see cref="IServiceCollection"/>.</summary>
[PublicAPI]
public static class ServiceCollectionExtensions
{
	private static readonly ProductInfoHeaderValue _userAgent;

	static ServiceCollectionExtensions()
	{
		var assemblyName = typeof(INordigenClient).Assembly.GetName();
		var assemblyShortName = assemblyName.Name ?? assemblyName.FullName.Split(',').First();
		_userAgent = new(assemblyShortName, assemblyName.Version?.ToString());
	}

	/// <summary>Adds all required services for <see cref="INordigenClient"/>.</summary>
	/// <param name="serviceCollection">The service collection in which to register the services.</param>
	/// <param name="configuration">The configuration to which to bind options models.</param>
	/// <param name="clock">Clock for accessing the current time.</param>
	/// <param name="dateTimeZoneProvider">Time zone provider for date and time serialization.</param>
	/// <returns>The <see cref="IHttpClientBuilder"/> for the <see cref="HttpClient"/> used by <see cref="INordigenClient"/>.</returns>
	public static IHttpClientBuilder AddNordigenDotNet(
		this IServiceCollection serviceCollection,
		IConfiguration configuration,
		IClock clock,
		IDateTimeZoneProvider dateTimeZoneProvider)
	{
		serviceCollection.TryAddSingleton(clock);
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

		serviceCollection.AddHttpClient<TokenDelegatingHandler>(ConfigureNordigenClient);

		return serviceCollection
			.AddSingleton<NordigenTokenCache>()
			.AddSingleton<NordigenJsonSerializerOptions>()
			.AddTransient<INordigenClient, NordigenClient>()
			.AddTransient<IAccountClient, AccountClient>(provider =>
			{
				var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(NordigenOptions.SectionName);
				var options = provider.GetRequiredService<NordigenJsonSerializerOptions>();
				return new(httpClient, options);
			})
			.AddTransient<IAgreementClient, AgreementClient>(provider =>
			{
				var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(NordigenOptions.SectionName);
				var options = provider.GetRequiredService<NordigenJsonSerializerOptions>();
				return new(httpClient, options);
			})
			.AddTransient<IInstitutionClient, InstitutionClient>(provider =>
			{
				var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(NordigenOptions.SectionName);
				var options = provider.GetRequiredService<NordigenJsonSerializerOptions>();
				return new(httpClient, options);
			})
			.AddTransient<IRequisitionClient, RequisitionClient>(provider =>
			{
				var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(NordigenOptions.SectionName);
				var options = provider.GetRequiredService<NordigenJsonSerializerOptions>();
				return new(httpClient, options);
			})
			.AddTransient(provider => provider.GetRequiredService<IOptionsMonitor<NordigenOptions>>().CurrentValue)
			.AddHttpClient(NordigenOptions.SectionName, ConfigureNordigenClient)
			.AddHttpMessageHandler<TokenDelegatingHandler>();
	}

	private static void ConfigureNordigenClient(IServiceProvider provider, HttpClient client)
	{
		client.BaseAddress = provider.GetRequiredService<IOptionsMonitor<NordigenOptions>>().CurrentValue.BaseAddress;
		client.DefaultRequestHeaders.UserAgent.Add(_userAgent);
	}
}
