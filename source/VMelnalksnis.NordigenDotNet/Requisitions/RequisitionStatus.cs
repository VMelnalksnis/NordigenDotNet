namespace VMelnalksnis.NordigenDotNet.Requisitions;

/// <summary>All possible states of a requisition.</summary>
public enum RequisitionStatus
{
	/// <summary>Requisition has been successfully created.</summary>
	Cr = 1,

	/// <summary>Account has been successfully linked to requisition.</summary>
	Ln,

	/// <summary>Access to account has expired as set in End User Agreement.</summary>
	Ex,

	/// <summary>SSN verification has failed.</summary>
	Rj,

	/// <summary>End-user is redirected to the financial institution for authentication.</summary>
	Ua,

	/// <summary>End-user is granting access to their account information.</summary>
	Ga,

	/// <summary>End-user is selecting accounts.</summary>
	Sa,

	/// <summary>End-user is giving consent at Nordigen's consent screen.</summary>
	Gc,
}
