using Application.Authorization;
using Application.DTOs.ViewModels;
using Application.Interfaces;
using Domain.Entities;
using Helpers.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Services;

namespace WebApi.Controllers.AdminDashboard
{ 
    public class HomeController : BaseMVCController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityService _identityService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IIdentityService identityService,
            IAuthenticatedUserService authenticatedUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _identityService = identityService;
            _authenticatedUserService = authenticatedUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("List", "DashboardLeads");

            var model = new LoginVM
            {
                return_url = returnUrl,
                ExternalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Home", new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/dashboard");

            var loginVM = new LoginVM
            {
                return_url = returnUrl,
                ExternalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if(remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from rxternal login provider: {remoteError}");
                return View(nameof(Login), loginVM);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if(info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login info");
                return View(nameof(Login), loginVM);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if(email == null)
            {
                TempData["ErrorTitle"] = $"Email claim not received from {info.ProviderDisplayName}";
                TempData["ErrorMessage"] = "Please contact administrator";
                return View("Error");
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null ||
                   (!(await _identityService.IsUserInRole(user, DefaultRoles.ADMIN.ToString())) &&
                    !(await _identityService.IsUserInRole(user, DefaultRoles.VIEW_ONLY.ToString()))))
                {
                    ModelState.AddModelError(string.Empty, "User not allowed to login");
                    return View(nameof(UnAuthorized));
                }

                var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);

                if (signInResult.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, false);

                    return LocalRedirect(returnUrl);
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // [Authorize(Policy = KeyValueConstants.AdminPolicy)]
        // [Authorize(Policy = KeyValueConstants.AdminPolicy)]
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        //[Authorize(Policy = KeyValueConstants.AdminPolicy)]
        public string Test()
        {
            var dd = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            var claims = HttpContext.User.Claims;
            var userClaims = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userid = _authenticatedUserService.UserId;
            var username = _authenticatedUserService.Username;
            return "I am here";
        }

        [AllowAnonymous]
        public IActionResult NotFound()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult UnAuthorized()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
