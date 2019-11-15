using System;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class SolutionPublicResult
    {
        public string Id { get; }

        public string Name { get; }

        public string OrganisationName { get; }

        public bool IsFoundation { get; }

        public string LastUpdated { get; }

        public PublicSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionPublicResult"/> class.
        /// </summary>
        public SolutionPublicResult(ISolution solution)
        {
            if (solution is null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            Id = solution.Id;
            Name = solution.Name;
            OrganisationName = solution.OrganisationName;

            //Canned Data --Todo
            IsFoundation = true;
            LastUpdated = DateTime.Today.ToString("dd-MMM-yyyy");

            Sections = new PublicSections(solution);
        }
    }
}
