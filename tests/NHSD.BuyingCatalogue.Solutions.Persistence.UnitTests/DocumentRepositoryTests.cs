using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.BuyingCatalogue.Contracts.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Persistence.Clients;
using NHSD.BuyingCatalogue.Solutions.Persistence.Repositories;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.UnitTests
{
    public sealed class DocumentRepositoryTests
    {
        private const string SampleUrl = "http://localhost/";
        private const string RoadMapIdentifier = "RoadMap";
        private const string DocumentIntegrationIdentifier = "Integration";
        private const string DocumentSolutionIdentifier = "Solution";
        private const string SolutionId = "Sln1";

        private Mock<IDocumentsAPIClient> _apiClientMock;
        private CancellationToken _cancellationToken;
        private Mock<ILogger<DocumentRepository>> _loggerMock;
        private Mock<ISettings> _settingsMock;

        [SetUp]
        public void Setup()
        {
            _apiClientMock = new Mock<IDocumentsAPIClient>();
            _apiClientMock.SetupAllProperties();

            _settingsMock = new Mock<ISettings>();
            _settingsMock.SetupGet(s => s.DocumentApiBaseUrl).Returns(SampleUrl);
            _settingsMock.SetupGet(s => s.DocumentRoadMapIdentifier).Returns(RoadMapIdentifier);
            _settingsMock.SetupGet(s => s.DocumentIntegrationIdentifier).Returns(DocumentIntegrationIdentifier);
            _settingsMock.SetupGet(s => s.DocumentSolutionIdentifier).Returns(DocumentSolutionIdentifier);

            _loggerMock = new Mock<ILogger<DocumentRepository>>();
            _cancellationToken = new CancellationToken();
        }

        [TestCase(new[] { "RoadMap.pdf" }, "RoadMap.pdf", null, null)]
        [TestCase(new[] { "NotMapped" }, null, null, null)]
        [TestCase(new[] { "Integration" }, null, "Integration", null)]
        [TestCase(new[] { "Solution.pdf" }, null, null, "Solution.pdf")]
        [TestCase(new[] { "RoadMap", "Integration.pdf", "Solution.pdf" }, "RoadMap", "Integration.pdf", "Solution.pdf")]
        public async Task ShouldGetDocumentsBySolutionId(
            string[] clientResult,
            string roadMapDocumentName,
            string integrationDocumentName,
            string solutionDocumentName)
        {
            _apiClientMock.Setup(api => api.DocumentsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResult);

            var sut = new DocumentRepository(_apiClientMock.Object, _settingsMock.Object, _loggerMock.Object);

            var result = await sut.GetDocumentResultBySolutionIdAsync(SolutionId, _cancellationToken)
                .ConfigureAwait(false);

            result.RoadMapDocumentName.Should().Be(roadMapDocumentName);
            result.IntegrationDocumentName.Should().Be(integrationDocumentName);
            result.SolutionDocumentName.Should().Be(solutionDocumentName);

            _apiClientMock.Verify(api => api.DocumentsAsync(SolutionId, CancellationToken.None), Times.Once);
        }

        [Test]
        public void ShouldSetBaseUrl()
        {
            var unused = new DocumentRepository(_apiClientMock.Object, _settingsMock.Object, _loggerMock.Object);
            _apiClientMock.Object.BaseAddress.AbsoluteUri.Should().Be(SampleUrl);
        }

        [Test]
        public async Task ShouldLogErrorReturnEmptyResultOnApiException()
        {
            _apiClientMock.Setup(api => api.DocumentsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ApiException("Api Failure", 500, "Response", null, null));
            var sut = new DocumentRepository(_apiClientMock.Object, _settingsMock.Object, _loggerMock.Object);

            var result = await sut.GetDocumentResultBySolutionIdAsync(SolutionId, _cancellationToken)
                .ConfigureAwait(false);
            result.RoadMapDocumentName.Should().BeNullOrEmpty();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Test]
        public async Task ShouldLogErrorReturnEmptyResultOnHttpRequestException()
        {
            _apiClientMock.Setup(api => api.DocumentsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException("message"));
            var sut = new DocumentRepository(_apiClientMock.Object, _settingsMock.Object, _loggerMock.Object);

            var result = await sut.GetDocumentResultBySolutionIdAsync(SolutionId, _cancellationToken)
                .ConfigureAwait(false);
            result.RoadMapDocumentName.Should().BeNullOrEmpty();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }
    }
}
