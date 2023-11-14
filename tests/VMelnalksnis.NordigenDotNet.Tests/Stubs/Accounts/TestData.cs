using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace VMelnalksnis.NordigenDotNet.Tests.Stubs.Accounts;

internal static class TestData
{
	private static readonly Type _namespaceType = typeof(TestData);
	private static readonly Assembly _assembly = _namespaceType.Assembly;

	internal static TestCase GetDetails => new(
		GetResourceStreamContent("GetDetails.json"));

	internal static TestCase GetTransactions => new(GetResourceStreamContent("GetTransactions.json"));

	private static string GetResourceStreamContent(string name)
	{
		var stream = _assembly.GetManifestResourceStream(_namespaceType, name);
		if (stream is not null)
		{
			StreamReader reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}

		var message = $"Could not find resource {name} in namespace {_namespaceType.Namespace}";
		throw new MissingManifestResourceException(message);
	}

	internal sealed record TestCase(string Result);
}
