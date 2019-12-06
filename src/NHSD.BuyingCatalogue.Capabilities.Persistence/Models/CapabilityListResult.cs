using System;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Capabilities.Persistence.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "False Positive")]
    internal sealed class CapabilityListResult : ICapabilityListResult
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsFoundation { get; set; }
    }
}
