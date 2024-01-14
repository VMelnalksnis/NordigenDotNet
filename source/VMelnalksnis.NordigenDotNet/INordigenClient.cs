using VMelnalksnis.NordigenDotNet.Accounts;
using VMelnalksnis.NordigenDotNet.Agreements;
using VMelnalksnis.NordigenDotNet.Institutions;
using VMelnalksnis.NordigenDotNet.Requisitions;

namespace VMelnalksnis.NordigenDotNet;

/// <summary>All available Nordigen APIs.</summary>
public interface INordigenClient
{
	/// <summary>Gets the accounts API client.</summary>
	IAccountClient Accounts { get; }

	/// <summary>Gets the agreements API client.</summary>
	IAgreementClient Agreements { get; }

	/// <summary>Gets the institutions API client.</summary>
	IInstitutionClient Institutions { get; }

	/// <summary>Gets the requisitions API client.</summary>
	IRequisitionClient Requisitions { get; }
}
