using Application.Features.Leads.Commands;
using Application.Features.Leads.Queries;
using Application.Features.Leads.Queries.GetUserLeadDetails;
using Helpers.Constants;
using Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using static Application.Features.Leads.Queries.GetUserLeadDetails.GetUserLeadDetails;
using static Application.Features.Leads.Queries.GetUserLeads;

namespace WebApi.Controllers.v1
{
    [Authorize(Policy = KeyValueConstants.UserPolicy)]
    public class LeadsController : BaseApiController
    {
        private const string baseRoute = "leads";

        /// <summary>
        /// Add new lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///      
        [HttpPost(baseRoute)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateNewLead(CreateLead.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        /// <summary>
        /// Get user leads list ordered by creation date decending
        /// </summary>
        /// <param name="page_number"></param>
        /// <param name="page_size"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet(baseRoute)]
        [ProducesResponseType(typeof(PaginatedResult<UserLeadsResponseDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserLeads([FromQuery]int page_number, [FromQuery]int page_size, [FromQuery] string name)
        {
            return Ok(await Mediator.Send(new GetUserLeads.Query(page_number, page_size, name)));
        }

        /// <summary>
        /// Get user lead details
        /// </summary>
        /// <param name="lead_id"></param>
        /// <returns></returns>
        [HttpGet(baseRoute + "/{lead_id}")]
        [ProducesResponseType(typeof(Result<UserLeadDetailsResponseDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserLeadDetails(long lead_id)
        {
            return Ok(await Mediator.Send(new GetUserLeadDetails.Query(lead_id)));
        }

    }
}
