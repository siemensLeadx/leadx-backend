using Application.Authorization;
using Application.Features.UserAccount.Commands;
using Application.Features.UserAccount.Queries;
using Helpers.Constants;
using Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    public class UsersController : BaseApiController
    {
        private const string baseRoute = "users";

        /// <summary>
        /// Register new user account
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Register(Register.Command command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Verify user account after registeration
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/verify")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> VerifyAccount(VerifyAccount.Command command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Login request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/login")]
        public async Task<IActionResult> Login(Login.Command command)
        {
            command.ip_address = GenerateIPAddress();
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Forget password request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/password/forget")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ForgetPassword(ForgetPassword.Command command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Reset password request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/password/reset")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ResetPassword(ResetPassword.Command command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(baseRoute)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateProfile(UpdateProfile.Command command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Get paginated list of users 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sort"></param>
        /// <param name="page_number"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        /// 
        //[HasPermission(Permissions.ViewUsers)]
        //[HttpGet(baseRoute)]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<IActionResult> GetUsersList([FromQuery] string name,
        //    [FromQuery] Dictionary<string, string> sort,
        //    [FromQuery]int page_number, [FromQuery]int page_size)
        //{
        //    var result = await Mediator.Send(new UsersList.Query(name, sort.GetValueOrDefault(SortKey.by),
        //        sort.GetValueOrDefault(SortKey.order), page_number, page_size));

        //    return Ok(result);
        //}

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        /// <summary>
        /// Get user notification list
        /// </summary>
        /// <param name="page_number"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        ///       
        [HttpGet(baseRoute + "/notifications")]
        [ProducesResponseType(typeof(PaginatedResult<UserNotificationResponseDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserNotifications([FromQuery] int page_number,
            [FromQuery] int page_size)
        {
            var result = await Mediator.Send(new GetUserNotifications.Query(page_number, page_size));

            return Ok(result);
        }
    }
}
