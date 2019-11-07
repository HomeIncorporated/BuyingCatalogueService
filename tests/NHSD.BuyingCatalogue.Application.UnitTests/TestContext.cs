using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Infrastructure.Mapping;
using NHSD.BuyingCatalogue.Application.SolutionList.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.SubmitForReview;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionClientApplicationTypes;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionPlugins;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.UnitTests
{
    internal class TestContext
    {
        public Mock<ICapabilityRepository> MockCapabilityRepository { get; private set; }

        public Mock<ISolutionRepository> MockSolutionRepository { get; private set; }

        public Mock<IMarketingDetailRepository> MockMarketingDetailRepository { get; private set; }

        public ListCapabilitiesHandler ListCapabilitiesHandler => (ListCapabilitiesHandler)_scope.ListCapabilitiesHandler;

        public ListSolutionsHandler ListSolutionsHandler => (ListSolutionsHandler)_scope.ListSolutionsHandler;

        public GetSolutionByIdHandler GetSolutionByIdHandler => (GetSolutionByIdHandler)_scope.GetSolutionByIdHandler;

        public UpdateSolutionSummaryHandler UpdateSolutionSummaryHandler => (UpdateSolutionSummaryHandler)_scope.UpdateSolutionSummaryHandler;

        public UpdateSolutionFeaturesHandler UpdateSolutionFeaturesHandler => (UpdateSolutionFeaturesHandler)_scope.UpdateSolutionFeaturesHandler;

        public SubmitSolutionForReviewHandler SubmitSolutionForReviewHandler => (SubmitSolutionForReviewHandler)_scope.SubmitSolutionForReviewHandler;

        public UpdateSolutionClientApplicationTypesHandler UpdateSolutionClientApplicationTypesHandler => (UpdateSolutionClientApplicationTypesHandler)_scope.UpdateSolutionClientApplicationTypesHandler;

        public UpdateSolutionBrowsersSupportedHandler UpdateSolutionBrowsersSupportedHandler => (UpdateSolutionBrowsersSupportedHandler)_scope.UpdateSolutionBrowsersSupportedHandler;

        public UpdateSolutionPluginsHandler UpdateSolutionPluginsHandler =>
            (UpdateSolutionPluginsHandler)_scope.UpdateSolutionPluginsHandler;

        private readonly Scope _scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);

            serviceCollection.AddSingleton<IMapper>(GetAutoMapper());
            serviceCollection.RegisterApplication();

            serviceCollection.AddTransient<IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>>, ListCapabilitiesHandler>();
            serviceCollection.AddTransient<IRequestHandler<ListSolutionsQuery, ListSolutionsResult>, ListSolutionsHandler>();
            serviceCollection.AddTransient<IRequestHandler<GetSolutionByIdQuery, ISolution>, GetSolutionByIdHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult>, UpdateSolutionSummaryHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidationResult>, UpdateSolutionFeaturesHandler>();
            serviceCollection.AddTransient<IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult>, SubmitSolutionForReviewHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionClientApplicationTypesCommand, UpdateSolutionClientApplicationTypesValidationResult>, UpdateSolutionClientApplicationTypesHandler>();
            serviceCollection.AddTransient<IRequestHandler<UpdateSolutionBrowsersSupportedCommand, UpdateSolutionBrowserSupportedValidationResult>, UpdateSolutionBrowsersSupportedHandler>();
            serviceCollection
                .AddTransient<IRequestHandler<UpdateSolutionPluginsCommand, UpdateSolutionPluginsValidationResult>,
                    UpdateSolutionPluginsHandler>();

            serviceCollection.AddSingleton<Scope>();
            _scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }

        private void RegisterDependencies(ServiceCollection serviceCollection)
        {
            MockCapabilityRepository = new Mock<ICapabilityRepository>();
            serviceCollection.AddSingleton<ICapabilityRepository>(MockCapabilityRepository.Object);
            MockSolutionRepository = new Mock<ISolutionRepository>();
            serviceCollection.AddSingleton<ISolutionRepository>(MockSolutionRepository.Object);
            MockMarketingDetailRepository = new Mock<IMarketingDetailRepository>();
            serviceCollection.AddSingleton<IMarketingDetailRepository>(MockMarketingDetailRepository.Object);
        }

        private IMapper GetAutoMapper()
        {
            var config = new MapperConfiguration(opts => opts.AddProfile(new AutoMapperProfile()));
            return config.CreateMapper();
        }

        private class Scope
        {
            public IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>> ListCapabilitiesHandler { get; }

            public IRequestHandler<ListSolutionsQuery, ListSolutionsResult> ListSolutionsHandler { get; }

            public IRequestHandler<GetSolutionByIdQuery, ISolution> GetSolutionByIdHandler { get; }

            public IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult> SubmitSolutionForReviewHandler { get; }

            public IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult> UpdateSolutionSummaryHandler { get; }

            public IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidationResult> UpdateSolutionFeaturesHandler { get; }

            public IRequestHandler<UpdateSolutionClientApplicationTypesCommand, UpdateSolutionClientApplicationTypesValidationResult> UpdateSolutionClientApplicationTypesHandler { get; }

            public IRequestHandler<UpdateSolutionBrowsersSupportedCommand, UpdateSolutionBrowserSupportedValidationResult> UpdateSolutionBrowsersSupportedHandler { get; }

            public IRequestHandler<UpdateSolutionPluginsCommand, UpdateSolutionPluginsValidationResult> UpdateSolutionPluginsHandler { get; }

            public Scope(IRequestHandler<ListCapabilitiesQuery, IEnumerable<ICapability>> listCapabilitiesHandler,
                IRequestHandler<ListSolutionsQuery, ListSolutionsResult> listSolutionsHandler,
                IRequestHandler<GetSolutionByIdQuery, ISolution> getSolutionByIdHandler,
                IRequestHandler<SubmitSolutionForReviewCommand, SubmitSolutionForReviewCommandResult> submitSolutionForReviewHandler,
                IRequestHandler<UpdateSolutionSummaryCommand, UpdateSolutionSummaryValidationResult> updateSolutionSummaryHandler,
                IRequestHandler<UpdateSolutionFeaturesCommand, UpdateSolutionFeaturesValidationResult> updateSolutionFeaturesHandler,
                IRequestHandler<UpdateSolutionClientApplicationTypesCommand, UpdateSolutionClientApplicationTypesValidationResult> updateSolutionClientApplicationTypesHandler,
                IRequestHandler<UpdateSolutionBrowsersSupportedCommand, UpdateSolutionBrowserSupportedValidationResult> updateSolutionBrowsersSupportedHandler,
                IRequestHandler<UpdateSolutionPluginsCommand, UpdateSolutionPluginsValidationResult> updateSolutionPluginsHandler)
            {
                ListCapabilitiesHandler = listCapabilitiesHandler;
                ListSolutionsHandler = listSolutionsHandler;
                GetSolutionByIdHandler = getSolutionByIdHandler;
                SubmitSolutionForReviewHandler = submitSolutionForReviewHandler;
                UpdateSolutionSummaryHandler = updateSolutionSummaryHandler;
                UpdateSolutionFeaturesHandler = updateSolutionFeaturesHandler;
                UpdateSolutionClientApplicationTypesHandler = updateSolutionClientApplicationTypesHandler;
                UpdateSolutionBrowsersSupportedHandler = updateSolutionBrowsersSupportedHandler;
                UpdateSolutionPluginsHandler = updateSolutionPluginsHandler;
            }
        }
    }
}
