using Microsoft.Extensions.DependencyInjection;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.SolutionList.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;

namespace NHSD.BuyingCatalogue.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CapabilityReader>();
            serviceCollection.AddTransient<SolutionListReader>();
            serviceCollection.AddTransient<SolutionReader>();
            serviceCollection.AddTransient<SolutionSummaryUpdater>();
            serviceCollection.AddTransient<SolutionFeaturesUpdater>();
            serviceCollection.AddTransient<SolutionClientApplicationUpdater>();
            serviceCollection.AddTransient<ClientApplicationPartialUpdater>();
            serviceCollection.AddTransient<UpdateSolutionSummaryValidator>();
            return serviceCollection;
        }
    }
}
