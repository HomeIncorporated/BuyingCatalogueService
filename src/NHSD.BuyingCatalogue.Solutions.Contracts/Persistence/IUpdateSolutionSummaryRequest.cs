namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IUpdateSolutionSummaryRequest
    {
        string SolutionId { get; }

        string Summary { get; }

        string Description { get; }

        string AboutUrl { get; }
    }
}
