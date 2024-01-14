using System.Collections.Generic;
using System.Text.Json.Serialization;

using VMelnalksnis.NordigenDotNet.Institutions;

namespace VMelnalksnis.NordigenDotNet.Agreements;

/// <summary>Details needed to create a new end-user agreement.</summary>
public record EndUserAgreementCreation
{
	/// <summary>Initializes a new instance of the <see cref="EndUserAgreementCreation"/> class.</summary>
	/// <param name="institutionId">An <see cref="Institution"/> id for this agreement.</param>
	/// <param name="maxHistoricalDays">Maximum number of days of transaction data to retrieve.</param>
	/// <param name="accessValidForDays">Number of days from acceptance that the access can be used.</param>
	/// <param name="accessScope">Level of information to access.</param>
	public EndUserAgreementCreation(
		string institutionId,
		int maxHistoricalDays,
		int accessValidForDays,
		List<string> accessScope)
	{
		InstitutionId = institutionId;
		MaxHistoricalDays = maxHistoricalDays;
		AccessValidForDays = accessValidForDays;
		AccessScope = accessScope;
	}

	/// <summary>Initializes a new instance of the <see cref="EndUserAgreementCreation"/> class with the default values.</summary>
	/// <param name="institutionId">An <see cref="Institution"/> id for this agreement.</param>
	public EndUserAgreementCreation(string institutionId)
		: this(institutionId, 90, 90, new() { "balances", "details", "transactions" })
	{
	}

	/// <summary>Gets an <see cref="Institution"/> id for this agreement.</summary>
	[JsonPropertyName("institution_id")]
	public string InstitutionId { get; }

	/// <summary>Gets maximum number of days of transaction data to retrieve.</summary>
	[JsonPropertyName("max_historical_days")]
	public int MaxHistoricalDays { get; }

	/// <summary>Gets number of days from acceptance that the access can be used.</summary>
	[JsonPropertyName("access_valid_for_days")]
	public int AccessValidForDays { get; }

	/// <summary>Gets level of information to access.</summary>
	[JsonPropertyName("access_scope")]
	public List<string> AccessScope { get; }
}
