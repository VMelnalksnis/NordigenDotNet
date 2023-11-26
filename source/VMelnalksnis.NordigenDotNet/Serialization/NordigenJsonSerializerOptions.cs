// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Serialization;

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
			.ConfigureForNodaTime(dateTimeZoneProvider);

		Context = new(options);
	}

	internal NordigenSerializationContext Context { get; }
}
