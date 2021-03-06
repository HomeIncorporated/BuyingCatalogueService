using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowserMobileFirst
{
    internal sealed class UpdateSolutionBrowserMobileFirstValidator : IValidator<UpdateSolutionBrowserMobileFirstCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionBrowserMobileFirstCommand command)
            => new RequiredValidator()
                .Validate(command.MobileFirstDesign, "mobile-first-design")
                .Result();
    }
}
