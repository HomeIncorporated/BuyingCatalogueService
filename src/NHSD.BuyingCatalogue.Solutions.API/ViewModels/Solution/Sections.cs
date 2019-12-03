using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class Sections
    {
        [JsonProperty("solution-description")]
        public SolutionDescriptionSection SolutionDescription { get; }

        public FeaturesSection Features { get; }

        [JsonProperty("client-application-types")]
        public ClientApplicationTypesSection ClientApplicationTypes { get; }


        [JsonProperty("contact-details")]
        public ContactDetailsSection ContactDetails { get; }

        [JsonProperty("capabilities")]
        public CapabilitiesSection Capabilities { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Sections"/> class.
        /// </summary>
        public Sections(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            SolutionDescription = new SolutionDescriptionSection(solution).IfPopulated();
            Features = new FeaturesSection(solution.Features).IfPopulated();
            ClientApplicationTypes = new ClientApplicationTypesSection(solution.ClientApplication).IfPopulated();

            ContactDetails = new ContactDetailsSection(solution.Contacts);
            Capabilities = new CapabilitiesSection(solution.Capabilities);
        }
    }
}