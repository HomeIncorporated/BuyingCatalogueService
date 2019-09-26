using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace NHSD.BuyingCatalogue.Testing.Data.Entities
{

    public sealed class OrganisationEntity : EntityBase
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string OrganisationUrl { get; set; }

        public string OdsCode { get; set; }

        public string PrimaryRoleId { get; set; }

        public Guid CrmRef { get; set; }

        protected override string InsertSql => $@"
        INSERT INTO [dbo].[Organisation]
        ([Id]
        ,[Name]
        ,[Summary]
        ,[OrganisationUrl]
        ,[OdsCode]
        ,[PrimaryRoleId]
        ,[CrmRef])

        VALUES
            ('{Id}'
            ,'{Name}'
            ,'{Summary}'
            ,'{OrganisationUrl}'
            ,'{OdsCode}'
            ,'{PrimaryRoleId}'
            ,'{CrmRef}')";

        public static async Task<IEnumerable<OrganisationEntity>> FetchAllAsync()
        {
            return await SqlRunner.FetchAllAsync<OrganisationEntity>($@"SELECT [Id]
                                    ,[Name]
                                    ,[Summary]
                                    ,[OrganisationUrl]
                                    ,[OdsCode]
                                    ,[PrimaryRoleId]
                                    ,[CrmRef]
                                FROM Organisation");
        }
    }
}