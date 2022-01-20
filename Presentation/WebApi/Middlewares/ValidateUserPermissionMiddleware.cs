using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Middlewares
{
    public class ValidateUserPermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidateUserPermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, SignInManager<AppUser> signInManager,
            IIdentityService identityService)
        {
            if (httpContext.Request.Path.Value.Contains("dashboard"))
            {
                var loggedUser = httpContext.User;

                if (loggedUser != null && loggedUser.Claims.Any())
                {
                    var userId = loggedUser.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
                    var userRole = loggedUser.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

                    if (userId != null && userRole != null)
                    {
                        var isUserInHisRole = await identityService.IsUserInRole(userId.Value, userRole.Value);

                        if (!isUserInHisRole)
                        {
                            await signInManager.SignOutAsync();
                            httpContext.Response.Redirect("/dashboard/en/Home/Login");
                            return;
                        }
                    }
                }
            }

            await _next(httpContext);
        }
    }
}
