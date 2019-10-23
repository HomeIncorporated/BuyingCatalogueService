using MediatR;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById
{
    /// <summary>
    /// Represents the query paramters for the get Solution by ID request.
    /// </summary>
    public sealed class GetSolutionByIdQuery : IRequest<Solution>
    {
        /// <summary>
        /// The key information to identify a <see cref="Solution"/>.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="GetSolutionByIdQuery"/> class.
        /// </summary>
        public GetSolutionByIdQuery(string id)
        {
            Id = id;
        }
    }
}
