using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.NordigenDotNet.Institutions;

/// <summary>Nordigen API client for reading institution information.</summary>
public interface IInstitutionClient
{
	/// <summary>Gets all institutions available in the specified country.</summary>
	/// <param name="countryCode">ISO 3166 two-character country code.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>All institutions available in the specified country.</returns>
	Task<List<Institution>> GetByCountry(string countryCode, CancellationToken cancellationToken = default);

	/// <summary>Gets the institution with the specified id.</summary>
	/// <param name="id">The id of the institution.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The institution with the specified id.</returns>
	Task<Institution> Get(string id, CancellationToken cancellationToken = default);
}
