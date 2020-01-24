using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class SupplierResult : ISupplierResult
    {
        public string SolutionId { get; set;  }
        public string Description { get; set; }
        public string Link { get; set; }
    }
}