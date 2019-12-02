using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class MarketingContactEntitySteps
    {
        [Given(@"MarketingContacts exist")]
        public async Task GivenMarketingContactsExist(Table table)
        {
            foreach (var contact in table.CreateSet<MarketingContactEntity>())
            {
                await contact.SetLastUpdated().InsertAsync();
            }
        }

        [Given(@"No contacts exist for solution (.*)")]
        [Then(@"No contacts exist for solution (.*)")]
        public async Task NoContactsExist(string solutionId)
        {
            var contacts = await MarketingContactEntity.FetchForSolutionAsync(solutionId);
            contacts.Should().BeEmpty();
        }

        [Then(@"Last Updated has updated on the MarketingContact for solution (.*)")]
        public async Task LastUpdatedHasUpdatedOnMarketingContact(string solutionId)
        {
            var contacts = await MarketingContactEntity.FetchForSolutionAsync(solutionId);

            var lastUpdated = contacts.Select(x => x.LastUpdated).ToList();
          
            var currentDateTime = DateTime.Now;
            var pastDateTime = currentDateTime.AddSeconds(-5);

            lastUpdated.FirstOrDefault().Should().BeOnOrAfter(pastDateTime);
            lastUpdated.FirstOrDefault().Should().BeOnOrAfter(currentDateTime);

            lastUpdated.Skip(1).FirstOrDefault().Should().BeOnOrAfter(pastDateTime);
            lastUpdated.Skip(1).FirstOrDefault().Should().BeOnOrAfter(currentDateTime);
        }

        [Then(@"MarketingContacts exist for solution (.*)")]
        public async Task ThenMarketingContactsExist(string solutionId, Table table)
        {
            var expected = table.CreateSet<MarketingContactEntity>().ToList();
            var contacts = await MarketingContactEntity.FetchForSolutionAsync(solutionId);
            contacts.Count().Should().Be(expected.Count());
            contacts.Should().BeEquivalentTo(expected, config => config.Excluding(c => c.LastUpdated).Excluding(c => c.LastUpdatedBy).Excluding(c => c.SolutionId));
        }
    }
}
