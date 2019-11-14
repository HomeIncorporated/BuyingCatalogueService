using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Models;

namespace NHSD.BuyingCatalogue.Persistence.Repositories
{
    /// <summary>
    /// Represents the data access layer for the <see cref="ICapabilityListResult"/> entity.
    /// </summary>
    public sealed class CapabilityRepository : ICapabilityRepository
	{
		/// <summary>
		/// Database connection factory to provide new connections.
		/// </summary>
		public IDbConnectionFactory DbConnectionFactory { get; }

		/// <summary>
		/// Initialises a new instance of the <see cref="CapabilityRepository"/> class.
		/// </summary>
		public CapabilityRepository(IDbConnectionFactory dbConnectionFactory)
		{
			DbConnectionFactory = dbConnectionFactory ?? throw new System.ArgumentNullException(nameof(dbConnectionFactory));
		}

        /// <summary>
        /// Gets a list of <see cref="ICapabilityListResult"/> objects.
        /// </summary>
        /// <returns>A task representing an operation to retrieve a list of <see cref="ICapabilityListResult"/> objects.</returns>
        public async Task<IEnumerable<ICapabilityListResult>> ListAsync(CancellationToken cancellationToken)
		{
			using (IDbConnection databaseConnection = await DbConnectionFactory.GetAsync(cancellationToken).ConfigureAwait(false))
			{
				const string sql = @"SELECT Capability.Id, 
											 Name, 
											 ISNULL(IsFoundation, 0) AS IsFoundation
									FROM	 Capability 
											 LEFT OUTER JOIN FrameworkCapabilities ON Capability.Id = FrameworkCapabilities.CapabilityId
                                    ORDER BY IsFoundation DESC, UPPER(Name) ASC";

				return (await databaseConnection.QueryAsync<CapabilityListResult>(sql).ConfigureAwait(false)).ToList();
			}
		}
	}
}
