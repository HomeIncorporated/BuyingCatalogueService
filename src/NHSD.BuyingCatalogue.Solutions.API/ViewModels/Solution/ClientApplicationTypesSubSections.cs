using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class ClientApplicationTypesSubSections
    {
        [JsonProperty("browser-based")]
        public BrowserBasedSection BrowserBased { get; }

        [JsonProperty("native-mobile")]
        public NativeMobileSection NativeMobile { get; }

        [JsonProperty("native-desktop")]
        public NativeDesktopSection NativeDesktop { get; }

        [JsonIgnore]
        public bool HasData => BrowserBased != null || NativeMobile != null || NativeDesktop != null;

        public ClientApplicationTypesSubSections(IClientApplication clientApplication)
        {
            BrowserBased = clientApplication?.ClientApplicationTypes?.Contains("browser-based") == true ?
                new BrowserBasedSection(clientApplication).IfPopulated() :
                null;
            NativeMobile = clientApplication?.ClientApplicationTypes?.Contains("native-mobile") == true
                ? new NativeMobileSection(clientApplication).IfPopulated()
                : null;
            NativeDesktop = clientApplication?.ClientApplicationTypes?.Contains("native-desktop") == true
                ? new NativeDesktopSection(clientApplication).IfPopulated()
                : null;
        }
    }
}
