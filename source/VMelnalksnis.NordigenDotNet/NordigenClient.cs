using VMelnalksnis.NordigenDotNet.Accounts;
using VMelnalksnis.NordigenDotNet.Agreements;
using VMelnalksnis.NordigenDotNet.Institutions;
using VMelnalksnis.NordigenDotNet.Requisitions;

namespace VMelnalksnis.NordigenDotNet;

/// <inheritdoc />
public sealed class NordigenClient : INordigenClient
{
	/// <summary>Initializes a new instance of the <see cref="NordigenClient"/> class.</summary>
	/// <param name="accounts">Accounts API client.</param>
	/// <param name="agreementClient">Agreements API client.</param>
	/// <param name="institutions">Institutions API client.</param>
	/// <param name="requisitions">Requisitions API client.</param>
	public NordigenClient(
		IAccountClient accounts,
		IAgreementClient agreementClient,
		IInstitutionClient institutions,
		IRequisitionClient requisitions)
	{
		Accounts = accounts;
		Agreements = agreementClient;
		Institutions = institutions;
		Requisitions = requisitions;
	}

	/// <inheritdoc />
	public IAccountClient Accounts { get; }

	/// <inheritdoc />
	public IAgreementClient Agreements { get; }

	/// <inheritdoc />
	public IInstitutionClient Institutions { get; }

	/// <inheritdoc />
	public IRequisitionClient Requisitions { get; }
}
