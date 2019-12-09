using System;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    public sealed class OrganisationEntitySteps
    {

        [Given(@"Organisations exist")]
        public static async Task GivenOrganisationsExist(Table table)
        {
            foreach (var organisation in table.CreateSet<OrganisationTable>())
            {
                await InsertOrganisationAsync(organisation).ConfigureAwait(false);
            }
        }

        private static async Task InsertOrganisationAsync(OrganisationTable organisationTable)
        {
            await OrganisationEntityBuilder.Create()
                .WithName(organisationTable.Name)
                .WithId(Guid.NewGuid())
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }

        private class OrganisationTable
        {
            public string Name { get; set; }
        }
    }
}
