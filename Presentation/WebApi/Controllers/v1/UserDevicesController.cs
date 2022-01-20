using Application.Features.UserAccount.Commands;
using Application.Features.UserAccount.Queries;
using Helpers.Constants;
using Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    
    public class UserDevicesController : BaseApiController
    {
        private const string baseRoute = "fcm-tokens";

        /// <summary>
        /// Add / update user device token
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(baseRoute)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUpdateDevice(AddUpdateUserDevice.Command command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Remove user device token
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete(baseRoute)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RemoveDevice(RemoveUserDevice.Command command)
        {
            return Ok(await Mediator.Send(command));
        }

        

        /// <summary>
        /// Get paginated list of user devices
        /// </summary>
        /// <param name="page_number"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        //[HttpGet(baseRoute)]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<IActionResult> GetUserDevices([FromQuery]int page_number, [FromQuery]int page_size)
        //{
        //    return Ok(await Mediator.Send(new GetUserDevices.Query(page_size, page_number)));
        //}
    }
}
