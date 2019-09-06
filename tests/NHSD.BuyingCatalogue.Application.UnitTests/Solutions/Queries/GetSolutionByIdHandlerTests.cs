using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Application.UnitTests.Data;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions.Queries
{
    [TestFixture]
    public sealed class GetSolutionByIdHandlerTests
    {
        private Mock<IHttpContextAccessor> _context;
        private Mock<ISolutionRepository> _repository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void SetUp()
        {
            _context = new Mock<IHttpContextAccessor>();
            _repository = new Mock<ISolutionRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task Handle_CallsRepository_Once()
        {
            //ARRANGE
            var testData = SolutionTestData.Default();

            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).Returns(() => Task.FromResult(testData));

            var testObject = new GetSolutionByIdHandler(_repository.Object, _mapper.Object);

            //ACT
            var _ = await testObject.Handle(new GetSolutionByIdQuery(_context.Object, testData.Id), CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.ByIdAsync(testData.Id, CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Handle_CallsMapper_Once()
        {
            //ARRANGE
            var testData = SolutionTestData.Default();

            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).Returns(() => Task.FromResult(testData));

            var testObject = new GetSolutionByIdHandler(_repository.Object, _mapper.Object);

            //ACT
            _ = await testObject.Handle(new GetSolutionByIdQuery(_context.Object, testData.Id), CancellationToken.None);

            //ASSERT
            _mapper.Verify(x => x.Map<SolutionByIdViewModel>(testData), Times.Once);
        }

        [Test]
        public async Task Handle_NoData_ReturnsEmpty()
        {
            //ARRANGE
            var testObject = new GetSolutionByIdHandler(_repository.Object, _mapper.Object);

            //ACT
            var result = await testObject.Handle(new GetSolutionByIdQuery(_context.Object, "does not exist"), CancellationToken.None);

            //ASSERT
            result.ShouldNotBeNull();
            result.Solution.ShouldBeNull();
        }

        [Test]
        public async Task Handle_Data_ReturnsData()
        {
            //ARRANGE
            var testData = SolutionTestData.Default();
            var mapRes = new SolutionByIdViewModel();

            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).Returns(() => Task.FromResult(testData));
            _mapper.Setup(x => x.Map<SolutionByIdViewModel>(testData))
              .Returns(mapRes);

            var testObject = new GetSolutionByIdHandler(_repository.Object, _mapper.Object);

            //ACT
            var result = await testObject.Handle(new GetSolutionByIdQuery(_context.Object, testData.Id), CancellationToken.None);

            //ASSERT
            result.Solution.ShouldBe(mapRes);
        }
    }
}