using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileMemoryAndStorage
{
    public class UpdateSolutionMobileMemoryStorageCommand : IRequest<RequiredMaxLengthResult>
    {
        public string Id { get; }
        public string MinimumMemoryRequirement { get; set; }
        public string Description { get; set; }

        public UpdateSolutionMobileMemoryStorageCommand(string id, string minimumMemoryRequirement, string description)
        {
            Id = id;
            MinimumMemoryRequirement = minimumMemoryRequirement;
            Description = description;
        }
    }
}