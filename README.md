[![Nuget](https://img.shields.io/nuget/v/VMelnalksnis.NordigenDotNet?label=NordigenDotNet)](https://www.nuget.org/packages/VMelnalksnis.NordigenDotNet/)
[![Nuget](https://img.shields.io/nuget/v/VMelnalksnis.NordigenDotNet.DependencyInjection?label=NordigenDotNet.DependencyInjection)](https://www.nuget.org/packages/VMelnalksnis.NordigenDotNet.DependencyInjection/)
[![Codecov](https://img.shields.io/codecov/c/github/VMelnalksnis/NordigenDotNet)](https://app.codecov.io/gh/VMelnalksnis/NordigenDotNet)
[![Run tests](https://github.com/VMelnalksnis/NordigenDotNet/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/VMelnalksnis/NordigenDotNet/actions/workflows/test.yml)

# NordigenDotNet
.NET client for the [Nordigen](https://nordigen.com/en/) API.

# Usage

A separate [NuGet package](https://www.nuget.org/packages/VMelnalksnis.NordigenDotNet.DependencyInjection/)
is provided for ASP.NET Core
([IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration)
and [IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection))
.
For use outside of ASP.NET Core, see the
[configuration](source/VMelnalksnis.NordigenDotNet.DependencyInjection/ServiceCollectionExtensions.cs).

1. Add configuration (for optional values see [options](source/VMelnalksnis.NordigenDotNet/NordigenOptions.cs))
   ```yaml
   "Nordigen": {
       "SecretId": "",
       "SecretKey": ""
   }
   ```

2. Register required services (see [tests](tests/VMelnalksnis.NordigenDotNet.DependencyInjection.Tests/ServiceCollectionExtensionsTests.cs))
   ```csharp
   serviceCollection
       .AddSingleton<IClock>(SystemClock.Instance)
       .AddSingleton(DateTimeZoneProviders.Tzdb)
       .AddNordigenDotNet(configuration);
   ```

3. (Optional) Configure retries
   with [Polly](https://github.com/App-vNext/Polly) ([NuGet package](https://www.nuget.org/packages/Microsoft.Extensions.Http.Polly))
   ```csharp
   serviceCollection
       .AddNordigenDotNet(configuration)
       .AddPolicyHandler(...);
   ```

4. Use `INordigenClient` to access all endpoints, or one of the specific clients defined in `INordigenClient`

# Known issues

1. Does not support the
   [premium endpoints](https://nordigen.com/en/docs/account-information/integration/parameters-and-responses/#/premium)
2. Due to incomplete documentation and differences between data returned by each bank,
   not all data returned by the Nordigen API might be captured.
   When using the client with a new institution, consider inspecting the raw data returned by Nordigen.
   If something is missing, please [create an issue](https://github.com/VMelnalksnis/NordigenDotNet/issues/new)
