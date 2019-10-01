using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NHSD.BuyingCatalogue.Application.Infrastructure;

namespace NHSD.BuyingCatalogue.Persistence.Infrastructure
{
    /// <summary>
    /// A factory to provide a new database connection.
    /// </summary>
    public sealed class DbConnectionFactory : IDbConnectionFactory
    {
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the database connection details.
        /// </summary>
        private string DefaultConnectionString => Configuration.BuyingCatalogueConnectionString();

        /// <summary>
        /// Initialises a new instance of the <see cref="DbConnectionFactory"/> class.
        /// </summary>
        public DbConnectionFactory(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Gets a new database connection.
        /// </summary>
        /// <returns>A new database connection.</returns>
        public async Task<IDbConnection> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(cancellationToken, new SqlConnectionStringBuilder(DefaultConnectionString));
        }

        /// <summary>
        /// Gets a new database connection.
        /// </summary>
        /// <returns>A new database connection.</returns>
        public async Task<IDbConnection> GetAsync(CancellationToken cancellationToken, DbConnectionStringBuilder connectionStringBuilder)
        {
            if (connectionStringBuilder is null)
            {
                throw new ArgumentNullException(nameof(connectionStringBuilder));
            }

            DbConnection connection = SqlClientFactory.Instance.CreateConnection();
            connection.ConnectionString = connectionStringBuilder.ConnectionString;

            await connection.OpenAsync(cancellationToken);

            return connection;
        }
    }
}
