using NHSD.BuyingCatalogue.Capabilities.Application.Domain;
using NHSD.BuyingCatalogue.Capabilities.Contracts;

namespace NHSD.BuyingCatalogue.Capabilities.Application.Queries.ListCapabilities
{
    /// <summary>
	/// Provides the view representation for the <see cref="Capability"/> entity.
	/// </summary>
	internal sealed class CapabilityDto : ICapability
    {
        /// <summary>
        /// Capability Reference
        /// </summary>
        public string CapabilityReference { get; set; }

        /// <summary>
        /// Version of the Capability
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Name of the capability.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A value to determine whether or not this instance is the criteria to form a foundation solution.
        /// </summary>
        public bool IsFoundation { get; set; }
    }
}
