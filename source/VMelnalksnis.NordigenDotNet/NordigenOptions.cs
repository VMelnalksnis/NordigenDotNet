using System;
using System.ComponentModel.DataAnnotations;

namespace VMelnalksnis.NordigenDotNet;

/// <summary>Options for configuring <see cref="INordigenClient"/>.</summary>
public sealed record NordigenOptions
{
	/// <summary>The name of the configuration section.</summary>
	public const string SectionName = "Nordigen";

	/// <summary>Gets the base address of the Nordigen API.</summary>
	[Required]
	public Uri BaseAddress { get; init; } = new("https://ob.nordigen.com/");

	/// <summary>Gets the secret ID used to create new access tokens.</summary>
	[Required]
	public string SecretId { get; init; } = null!;

	/// <summary>Gets the secret key used to create new access tokens.</summary>
	[Required]
	public string SecretKey { get; init; } = null!;

	/// <summary>Gets the factor by which to divide the expiration time for access and refresh tokens.</summary>
	public double ExpirationFactor { get; init; } = 2d;
}
