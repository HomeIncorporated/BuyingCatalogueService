using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.BuyingCatalogue.Solutions.API.Controllers.ClientApplication.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Queries;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.API.UnitTests.ClientApplications.NativeMobile
{
    [TestFixture]
    public sealed class NativeMobileControllerTests
    {
        private Mock<IMediator> _mockMediator;

        private NativeMobileController _nativeMobileController;

        private const string SolutionId = "Sln1";

        [SetUp]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _nativeMobileController = new NativeMobileController(_mockMediator.Object);
        }

        [Test]
        public async Task ShouldReturnEmpty()
        {
            var result = (await _nativeMobileController.GetNativeMobileAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var nativeMobileResult = result.Value as NativeMobileResult;
            nativeMobileResult.Should().NotBeNull();
            nativeMobileResult.NativeMobileSections.Should().NotBeNull();

            var mobileOperatingSystems = nativeMobileResult.NativeMobileSections.MobileOperatingSystems;
            AssertSectionMandatoryAndComplete(mobileOperatingSystems, true, false);

            var mobileFirst = nativeMobileResult.NativeMobileSections.MobileFirst;
            AssertSectionMandatoryAndComplete(mobileFirst, true, false);

            var mobileMemoryStorage = nativeMobileResult.NativeMobileSections.MobileMemoryStorage;
            AssertSectionMandatoryAndComplete(mobileMemoryStorage, true, false);

            var mobileConnectionDetails = nativeMobileResult.NativeMobileSections.MobileConnectionDetails;
            AssertSectionMandatoryAndComplete(mobileConnectionDetails, false, false);

            var mobileComponentDeviceCapabilities =
                nativeMobileResult.NativeMobileSections.MobileComponentsDeviceCapabilities;
            AssertSectionMandatoryAndComplete(mobileComponentDeviceCapabilities, false, false);

            var mobileHardwareRequirements = nativeMobileResult.NativeMobileSections.MobileHardwareRequirements;
            AssertSectionMandatoryAndComplete(mobileHardwareRequirements, false, false);

            var mobileThirdParty = nativeMobileResult.NativeMobileSections.MobileThirdPartySection;
            AssertSectionMandatoryAndComplete(mobileThirdParty, false, false);

            var mobileAdditionalInformation = nativeMobileResult.NativeMobileSections.MobileAdditionalInformation;
            AssertSectionMandatoryAndComplete(mobileAdditionalInformation, false, false);
        }

        [Test]
        public async Task ShouldGetNativeMobileStaticData()
        {
            var nativeMobileResult =
                await GetNativeMobileSectionAsync(Mock.Of<IClientApplication>()).ConfigureAwait(false);

            nativeMobileResult.Should().NotBeNull();
            nativeMobileResult.NativeMobileSections.Should().NotBeNull();

            var mobileOperatingSystems = nativeMobileResult.NativeMobileSections.MobileOperatingSystems;
            AssertSectionMandatoryAndComplete(mobileOperatingSystems, true, false);

            var mobileFirst = nativeMobileResult.NativeMobileSections.MobileFirst;
            AssertSectionMandatoryAndComplete(mobileFirst, true, false);

            var mobileMemoryStorage = nativeMobileResult.NativeMobileSections.MobileMemoryStorage;
            AssertSectionMandatoryAndComplete(mobileMemoryStorage, true, false);

            var mobileConnectionDetails = nativeMobileResult.NativeMobileSections.MobileConnectionDetails;
            AssertSectionMandatoryAndComplete(mobileConnectionDetails,false, false);

            var mobileComponentDeviceCapabilities =
                nativeMobileResult.NativeMobileSections.MobileComponentsDeviceCapabilities;
            AssertSectionMandatoryAndComplete(mobileComponentDeviceCapabilities, false, false);

            var mobileHardwareRequirements = nativeMobileResult.NativeMobileSections.MobileHardwareRequirements;
            AssertSectionMandatoryAndComplete(mobileHardwareRequirements, false, false);

            var mobileThirdParty = nativeMobileResult.NativeMobileSections.MobileThirdPartySection;
            AssertSectionMandatoryAndComplete(mobileThirdParty, false, false);

            var mobileAdditionalInformation = nativeMobileResult.NativeMobileSections.MobileAdditionalInformation;
            AssertSectionMandatoryAndComplete(mobileAdditionalInformation, false, false);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGetMobileOperatingSystemsIsRequired(bool isOperatingSystemsEmpty)
        {
            var nativeMobileResult = await GetNativeMobileSectionAsync(Mock.Of<IClientApplication>(c =>
                c.MobileOperatingSystems ==
                Mock.Of<IMobileOperatingSystems>(m => m.OperatingSystems == (isOperatingSystemsEmpty ? new HashSet<string>() {"IOS"} : new HashSet<string>())))).ConfigureAwait(false);

            nativeMobileResult.NativeMobileSections.MobileOperatingSystems.Status.Should()
                .Be(isOperatingSystemsEmpty ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(false, false, false, false)]
        [TestCase(false, false, true, true)]
        [TestCase(false, true, false, true)]
        [TestCase(true, false, false, true)]
        [TestCase(true, true, false, true)]
        [TestCase(true, true, true, true)]
        [TestCase(false, true, true, true)]
        [TestCase(true, false, true, true)]
        public async Task ShouldGetMobileConnectionDetailsIsRequired(bool hasConnectionType, bool hasConnectionSpeed, bool hasDescription, bool expected)
        {
            var nativeMobileResult = await GetNativeMobileSectionAsync(Mock.Of<IClientApplication>(c =>
                c.MobileConnectionDetails ==
                Mock.Of<IMobileConnectionDetails>(m =>
                    m.ConnectionType == (hasConnectionType ? new HashSet<string>() { "3G" } : new HashSet<string>()) &&
                    m.MinimumConnectionSpeed  == (hasConnectionSpeed ? "1GBps" : null) &&
                    m.Description == (hasDescription ? "A description" : null)
                    ))).ConfigureAwait(false);

            nativeMobileResult.NativeMobileSections.MobileConnectionDetails.Status.Should()
                .Be(expected ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(false, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(false, false, false)]
        [TestCase(true, true, true)]
        [TestCase(false, true, false)]
        public async Task ShouldGetMobileMemoryAndStorageIsComplete(bool hasMinimumMemoryRequirement, bool hasDescription, bool expected)
        {
            var nativeMobileResult = await GetNativeMobileSectionAsync(Mock.Of<IClientApplication>(c =>
                c.MobileMemoryAndStorage ==
                Mock.Of<IMobileMemoryAndStorage>(m =>
                    m.MinimumMemoryRequirement == (hasMinimumMemoryRequirement ? "1GB" : null) &&
                    m.Description == (hasDescription ? "A description" : null)
                ))).ConfigureAwait(false);

            nativeMobileResult.NativeMobileSections.MobileMemoryStorage.Status.Should()
                .Be(expected ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(null, false)]
        [TestCase("Some Hardware", true)]
        public async Task ShouldGetNativeMobileHardwareRequirementIsComplete(string hardware, bool isComplete)
        {
            var nativeMobileResult = await GetNativeMobileSectionAsync(Mock.Of<IClientApplication>(c => c.NativeMobileHardwareRequirements == hardware))
                .ConfigureAwait(false);

            nativeMobileResult.NativeMobileSections.MobileHardwareRequirements.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase("     ", "    ", false)]
        [TestCase(null, null, false)]
        [TestCase("Components", null, true)]
        [TestCase(null, "Capabilities", true)]
        [TestCase("Components", "Capabilities", true)]
        public async Task ShouldGetNativeMobileMobileThirdPartyIsComplete(string component, string capability,
            bool isComplete)
        {
            var nativeMobileResult = await GetNativeMobileSectionAsync(
                    Mock.Of<IClientApplication>(c =>
                        c.MobileThirdParty == Mock.Of<IMobileThirdParty>(m =>
                            m.ThirdPartyComponents == component && m.DeviceCapabilities == capability)))
                .ConfigureAwait(false);

            nativeMobileResult.NativeMobileSections.MobileThirdPartySection.Status.Should()
                .Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        [TestCase(null, false)]
        [TestCase("Some additional information", true)]
        public async Task ShouldGetNativeMobileAdditionalInformationIsComplete(string additionalInformation, bool isComplete)
        {
            var nativeMobileResult = await GetNativeMobileSectionAsync(Mock.Of<IClientApplication>(c => c.NativeMobileAdditionalInformation == additionalInformation))
                .ConfigureAwait(false);

            nativeMobileResult.NativeMobileSections.MobileAdditionalInformation.Status.Should().Be(isComplete ? "COMPLETE" : "INCOMPLETE");
        }

        private async Task<NativeMobileResult> GetNativeMobileSectionAsync(IClientApplication clientApplication)
        {
            _mockMediator.Setup(m =>
                    m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientApplication);

            var result =
                (await _nativeMobileController.GetNativeMobileAsync(SolutionId).ConfigureAwait(false)) as ObjectResult;
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            _mockMediator.Verify(
                m => m.Send(It.Is<GetClientApplicationBySolutionIdQuery>(q => q.Id == SolutionId),
                    It.IsAny<CancellationToken>()), Times.Once);

            return result.Value as NativeMobileResult;
        }

        private static void AssertSectionMandatoryAndComplete(DashboardSection section, bool shouldBeMandatory, bool shouldBeComplete)
        {
            section.Status.Should().Be(shouldBeComplete ? "COMPLETE" : "INCOMPLETE");
            section.Requirement.Should().Be(shouldBeMandatory ? "Mandatory" : "Optional");
        }
    }
}
