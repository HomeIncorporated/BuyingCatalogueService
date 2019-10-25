using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.API.IntegrationTests.Support;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    internal sealed class EditSupportedBrowserSteps
    {
        private const string SupportedBrowserUrl = "http://localhost:8080/api/v1/solutions/{0}/sections/browsers-supported";

        private readonly ScenarioContext _context;

        private readonly Response _response;

        public EditSupportedBrowserSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [When(@"a PUT request is made to update solution (.*) browsers-supported section")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowsers_SupportedSection(string solutionId, Table table)
        {
            var content = table.CreateInstance<SupportedBrowsersTable>();
            _response.Result = await Client.PutAsJsonAsync(string.Format(SupportedBrowserUrl, solutionId), new SupportedBrowserPayload { BrowsersSupported = content.BrowsersSupported, MobileResponsive = content.MobileResponsive});

        }

        [When(@"a PUT request is made to update solution browsers-supported section with no solution id")]
        public async Task WhenAPUTRequestIsMadeToUpdateSolutionBrowsers_SupportedSectionWithNoSolutionId(Table table)
        {
            await WhenAPUTRequestIsMadeToUpdateSolutionSlnBrowsers_SupportedSection(" ", table);
        }


        private class SupportedBrowserPayload
        {
            [JsonProperty("supported-browsers")]
            public List<string> BrowsersSupported { get; set; }

            [JsonProperty("mobile-responsive")]
            public string MobileResponsive { get; set; }
        }

        private class SupportedBrowsersTable
        {
            public List<string> BrowsersSupported { get; set; }

            public string MobileResponsive { get; set; }
        }
    }
}