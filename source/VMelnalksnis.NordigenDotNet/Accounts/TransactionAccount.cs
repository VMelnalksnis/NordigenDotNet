namespace VMelnalksnis.NordigenDotNet.Accounts;

/// <summary>An account that can be referenced from a <see cref="Transaction"/>.</summary>
public record TransactionAccount
{
	/// <summary>Gets or sets the IBAN of the account.</summary>
	public string Iban { get; set; } = null!;
}
