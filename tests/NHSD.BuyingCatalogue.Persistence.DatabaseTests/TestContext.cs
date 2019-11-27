using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Data;
using NHSD.BuyingCatalogue.Data.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;
using NHSD.BuyingCatalogue.SolutionLists.Persistence;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Solutions.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    internal class TestContext
    {
        public IMarketingContactRepository MarketingContactRepository => _scope.MarketingContactRepository;

        public ISolutionCapabilityRepository SolutionCapabilityRepository => _scope.SolutionCapabilityRepository;

        public ISolutionDetailRepository SolutionDetailRepository => _scope.SolutionDetailRepository;

        public ISolutionListRepository SolutionListRepository => _scope.SolutionListRepository;

        public ISolutionRepository SolutionRepository => _scope.SolutionRepository;

        public IDbConnector DbConnector => _scope.DbConnector;

        private readonly Scope _scope;

        public TestContext()
        {
            var serviceCollection = new ServiceCollection();
            var settings = new Mock<ISettings>();
            settings.Setup(s => s.ConnectionString).Returns(ConnectionStrings.ServiceConnectionString());
            serviceCollection.AddSingleton(settings.Object);

            serviceCollection.RegisterData();
            serviceCollection.RegisterSolutionsPersistence();
            serviceCollection.RegisterSolutionListPersistence();

            serviceCollection.AddSingleton<Scope>();

            _scope = serviceCollection.BuildServiceProvider().GetService<Scope>();
        }
        
        private class Scope
        {
            public IMarketingContactRepository MarketingContactRepository { get; }

            public ISolutionCapabilityRepository SolutionCapabilityRepository { get; }

            public ISolutionDetailRepository SolutionDetailRepository { get; }

            public ISolutionListRepository SolutionListRepository { get; }

            public ISolutionRepository SolutionRepository { get; }

            public IDbConnector DbConnector { get; }

            public Scope(IMarketingContactRepository marketingContactRepository,
                ISolutionCapabilityRepository solutionCapabilityRepository,
                ISolutionDetailRepository solutionDetailRepository,
                ISolutionListRepository solutionListRepository,
                ISolutionRepository solutionRepository,
                IDbConnector dbConnector)
            {
                MarketingContactRepository = marketingContactRepository;
                SolutionCapabilityRepository = solutionCapabilityRepository;
                SolutionDetailRepository = solutionDetailRepository;
                SolutionListRepository = solutionListRepository;
                SolutionRepository = solutionRepository;
                DbConnector = dbConnector;
            }
        }
    }
}
