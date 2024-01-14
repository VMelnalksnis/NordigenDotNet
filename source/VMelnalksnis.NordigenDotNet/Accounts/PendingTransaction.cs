using VMelnalksnis.NordigenDotNet.Institutions;

namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>A transaction that is still pending.</summary>
public record PendingTransaction : Transaction
{
	/// <summary>Gets or sets a unique transaction id created by the <see cref="Institution"/>.</summary>
	public string? TransactionId { get; set; }
}
