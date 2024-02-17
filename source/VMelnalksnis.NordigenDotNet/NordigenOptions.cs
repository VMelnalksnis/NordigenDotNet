using System;
using System.ComponentModel.DataAnnotations;
#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace VMelnalksnis.NordigenDotNet;

/// <summary>Options for configuring <see cref="INordigenClient"/>.</summary>
#if NET6_0_OR_GREATER
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
public sealed record NordigenOptions
{
	/// <summary>The name of the configuration section.</summary>
	public const string SectionName = "Nordigen";

	/// <summary>Gets or sets the base address of the Nordigen API.</summary>
	[Required]
	public Uri BaseAddress { get; set; } = new("https://bankaccountdata.gocardless.com/");

	/// <summary>Gets or sets the secret ID used to create new access tokens.</summary>
	[Required]
	public string SecretId { get; set; } = null!;

	/// <summary>Gets or sets the secret key used to create new access tokens.</summary>
	[Required]
	public string SecretKey { get; set; } = null!;

	/// <summary>Gets or sets the factor by which to divide the expiration time for access and refresh tokens.</summary>
	public double ExpirationFactor { get; set; } = 2d;
}
