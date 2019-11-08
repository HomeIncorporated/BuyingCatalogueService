using System.Collections.Generic;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionFeatures;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public class UpdateSolutionFeaturesResult
    {
        public UpdateSolutionFeaturesResult(UpdateSolutionFeaturesValidationResult updateSolutionFeaturesValidationResult)
        {
            MaxLength = updateSolutionFeaturesValidationResult.MaxLength;
        }

        public HashSet<string> MaxLength { get; }
    }
}