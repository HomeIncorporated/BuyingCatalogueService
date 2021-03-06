using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.SolutionDescription
{
    [Binding]
    internal sealed class SolutionDescriptionSectionSteps
    {
        private readonly Response _response;

        public SolutionDescriptionSectionSteps(Response response)
        {
            _response = response;
        }

        [Then(@"the solution solution-description section does not contain (link|summary|description)")]
        public async Task ThenTheSolutionDoesNotContainLink(string field)
        {
            var content = await _response.ReadBody().ConfigureAwait(false);
            content.SelectToken($"sections.solution-description.answers.{field}").Should().BeNull();
        }
    }
}
