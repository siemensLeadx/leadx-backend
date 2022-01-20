using Application.DTOs.Responses;
using Application.Features.LookupData;
using Helpers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    public class LookupsController : BaseApiController
    {
        private const string baseRoute = "lookups";

        /// <summary>
        /// Get business opportunities lookup data
        /// </summary>
        /// <returns></returns>
        [HttpGet(baseRoute + "/business_opportunities")]
        [ProducesResponseType(typeof(Result<IEnumerable<LookupResponseDTO>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBusinessOpportunities()
        {
            var result = await Mediator.Send(new GetBusinessOpportunities.Query());

            return Ok(result);
        }

        /// <summary>
        /// Get customer status lookup data
        /// </summary>
        /// <returns></returns>
        [HttpGet(baseRoute + "/customer_status")]
        [ProducesResponseType(typeof(Result<IEnumerable<LookupResponseDTO>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerStatus()
        {
            var result = await Mediator.Send(new GetCustomerStatuses.Query());

            return Ok(result); 
        }


        /// <summary>
        /// Get lead devices lookup data
        /// </summary>
        /// <returns></returns>
        [HttpGet(baseRoute + "/devices")]
        [ProducesResponseType(typeof(Result<IEnumerable<LookupResponseDTO>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetNeededDevices()
        {
            var result = await Mediator.Send(new GetNeededDevices.Query());

            return Ok(result);
        }
    }
}
