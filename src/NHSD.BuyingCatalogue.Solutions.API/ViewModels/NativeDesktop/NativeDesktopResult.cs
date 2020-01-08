using System;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop
{
    public sealed class NativeDesktopResult
    {
        [JsonProperty("sections")]
        public NativeDesktopSections NativeDesktopSections { get; }

        public NativeDesktopResult(IClientApplication clientApplication)
        {
            NativeDesktopSections = new NativeDesktopSections(clientApplication);
        }
    }

    public sealed class NativeDesktopSections
    {
        [JsonProperty("native-desktop-operating-systems")]
        public DashboardSection OperatingSystems { get; }

        [JsonProperty("native-desktop-connection-details")]
        public DashboardSection ConnectionDetails { get; }

        [JsonProperty("native-desktop-memory-and-storage")]
        public DashboardSection MemoryAndStorage { get; }

        [JsonProperty("native-desktop-third-party")]
        public DashboardSection ThirdParty { get; }
        
        [JsonProperty("native-desktop-hardware-requirements")]
        public DashboardSection HardwareRequirements { get; }

        [JsonProperty("native-desktop-additional-information")]
        public DashboardSection AdditionalInformation { get; }

        public NativeDesktopSections(IClientApplication clientApplication)
        {
            OperatingSystems = DashboardSection.Mandatory(false);
            ConnectionDetails = DashboardSection.Mandatory(!String.IsNullOrWhiteSpace(clientApplication?.NativeDesktopMinimumConnectionSpeed));
            MemoryAndStorage = DashboardSection.Mandatory(false);
            ThirdParty = DashboardSection.Optional(false);
            HardwareRequirements = DashboardSection.Optional(!String.IsNullOrWhiteSpace(clientApplication?.NativeDesktopHardwareRequirements));
            AdditionalInformation = DashboardSection.Optional(false);
        }
    }
}
