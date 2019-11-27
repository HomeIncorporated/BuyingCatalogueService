using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowsersSupported;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class BrowsersSupportedController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowsersSupportedController"/> class.
        /// </summary>
        public BrowsersSupportedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the browsers supported for the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to retrieve the details of the browsers supported section.</returns>
        [HttpGet]
        [Route("{id}/sections/browsers-supported")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBrowsersSupportedAsync([FromRoute][Required]string id)
        {
            var solution = await _mediator.Send(new GetSolutionByIdQuery(id));
            return solution == null ? (ActionResult)new NotFoundResult() : Ok(new GetBrowsersSupportedResult(solution.ClientApplication));
        }

        /// <summary>
        /// Updates the browsers supported of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <param name="updateSolutionBrowsersSupportedViewModel">The details of the supported browsers.</param>
        /// <returns>A task representing an operation to update the details of the browser supported section.</returns>
        [HttpPut]
        [Route("{id}/sections/browsers-supported")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateBrowsersSupportedAsync([FromRoute][Required]string id, [FromBody][Required]UpdateSolutionBrowsersSupportedViewModel updateSolutionBrowsersSupportedViewModel)
        {
            var validationResult = await _mediator.Send(new UpdateSolutionBrowsersSupportedCommand(id, updateSolutionBrowsersSupportedViewModel));

            return validationResult.IsValid ? (ActionResult)new NoContentResult()
                : BadRequest(new UpdateSolutionBrowserSupportedResult(validationResult));
        }
    }
}