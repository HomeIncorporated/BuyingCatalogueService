using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Solutions.Domain
{
    internal class ClientApplicationDto : IClientApplication
    {
        public HashSet<string> ClientApplicationTypes { get; set; } = new HashSet<string>();

        public HashSet<string> BrowsersSupported { get; set; } = new HashSet<string>();

        public bool? MobileResponsive { get; set; }
    }
}
