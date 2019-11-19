using System.Linq;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    public sealed class SupplierEntitySteps
    {
        [Given(@"Suppliers exist")]
        public async Task GivenSuppliersExist(Table table)
        {
            foreach (var supplier in table.CreateSet<SupplierTable>())
            {
                await InsertSupplierAsync(supplier);
            }
        }

        private async Task InsertSupplierAsync(SupplierTable supplierTable)
        {
            var organisations = (await OrganisationEntity.FetchAllAsync()).ToList();

            await SupplierEntityBuilder.Create()
                .WithId(supplierTable.Id)
                .WithOrganisation(organisations.First(o => o.Name == supplierTable.OrganisationName).Id)
                .WithName(supplierTable.Id)
                .Build()
                .InsertAsync();
        }

        private class SupplierTable
        {
            public string Id { get; set; }
            public string OrganisationName { get; set; }
        }
    }
}