using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Contracts.Solutions;
using NHSD.BuyingCatalogue.Solutions.API.Controllers;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests
{
    [TestFixture]
    public sealed class BrowserBasedControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private BrowserBasedController _browserBasedController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _browserBasedController = new BrowserBasedController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            var result = (await _browserBasedController.GetBrowserBasedAsync(SolutionId)) as NotFoundResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task ShouldGetBrowserBasedStaticData()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>());

            browserBasedResult.Should().NotBeNull();
            browserBasedResult.BrowserBasedDashboardSections.Should().NotBeNull();

            var browsersSupportedSection = browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection;
            AssertSectionMandatoryAndComplete(browsersSupportedSection, true, false);

            var plugInsSection = browserBasedResult.BrowserBasedDashboardSections.PluginsOrExtensionsSection;
            AssertSectionMandatoryAndComplete(plugInsSection, true, false);

            var connectivitySection = browserBasedResult.BrowserBasedDashboardSections.ConnectivityAndResolutionSection;
            AssertSectionMandatoryAndComplete(connectivitySection, true, false);

            var hardwareSection = browserBasedResult.BrowserBasedDashboardSections.HardwareRequirementsSection;
            AssertSectionMandatoryAndComplete(hardwareSection, false, false);

            var additionalSection = browserBasedResult.BrowserBasedDashboardSections.AdditionalInformationSection;
            AssertSectionMandatoryAndComplete(additionalSection, false, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullClientApplication()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>());
            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullBrowsersSupported()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>()));

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteEmptyBrowsersSupported()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.BrowsersSupported == new HashSet<string>())));

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteNullMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.BrowsersSupported == new HashSet<string>{ "A" })));

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteBrowsersSupportedNullAndMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.MobileResponsive == false)));

            AssertBrowsersSupportedSectionComplete(browserBasedResult, false);
        }

        [Test]
        public async Task ShouldGetBrowserBasedCalculateCompleteBrowsersSupportedAndMobileResponsive()
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication == Mock.Of<IClientApplication>(c =>
                    c.BrowsersSupported == new HashSet<string>{ "A" } && c.MobileResponsive == false)));

            AssertBrowsersSupportedSectionComplete(browserBasedResult, true);
        }

        [TestCase(null, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public async Task ShouldGetBrowserBasedCalculateCompletePluginRequired(bool? pluginRequired, bool complete)
        {
            var browserBasedResult = await GetBrowserBasedSectionAsync(Mock.Of<ISolution>(s =>
                s.ClientApplication.Plugins == Mock.Of<IPlugins>(c => c.Required == pluginRequired && c.AdditionalInformation == null)));

            AssertPluginsSectionComplete(browserBasedResult, complete);
        }


        private async Task<BrowserBasedResult> GetBrowserBasedSectionAsync(ISolution solution)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(solution);

            var result = (await _browserBasedController.GetBrowserBasedAsync(SolutionId)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetSolutionByIdQuery>(q => q.Id == SolutionId), It.IsAny<CancellationToken>()),
                Times.Once);
            return result.Value as BrowserBasedResult;
        }

        private void AssertBrowsersSupportedSectionComplete(BrowserBasedResult browserBasedResult, bool shouldBeComplete)
        {
            browserBasedResult.BrowserBasedDashboardSections.BrowsersSupportedSection.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
        }

        private void AssertPluginsSectionComplete(BrowserBasedResult browserBasedResult, bool shouldBeComplete)
        {
            browserBasedResult.BrowserBasedDashboardSections.PluginsOrExtensionsSection.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
        }

        private void AssertSectionMandatoryAndComplete(BrowserBasedDashboardSection section, bool shouldBeMandatory, bool shouldBeComplete)
        {
            section.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
            section.Requirement.Should().Be(shouldBeMandatory ? "Mandatory" : "Optional");
        }
    }
}