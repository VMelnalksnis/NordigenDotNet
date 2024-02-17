using System.Text.Json;
#if NET6_0
using System.Text.Json.Serialization;
#endif

using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace VMelnalksnis.NordigenDotNet.Serialization;

/// <summary><see cref="JsonSerializerOptions"/> for <see cref="INordigenClient"/>.</summary>
public sealed class NordigenJsonSerializerOptions
{
	/// <summary>Initializes a new instance of the <see cref="NordigenJsonSerializerOptions"/> class.</summary>
	/// <param name="dateTimeZoneProvider">Time zone provider for date and time serialization.</param>
	public NordigenJsonSerializerOptions(IDateTimeZoneProvider dateTimeZoneProvider)
	{
		var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
		{
#if NET6_0
			Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
#endif
		}.ConfigureForNodaTime(dateTimeZoneProvider);

		Context = new(options);
	}

	internal NordigenSerializationContext Context { get; }
}
