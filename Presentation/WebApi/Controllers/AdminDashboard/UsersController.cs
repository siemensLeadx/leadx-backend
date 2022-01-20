using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.LookupData;
using Application.Features.UserAccount.Commands;
using Application.Features.UserAccount.Queries;
using Application.Interfaces;
using Helpers.Constants;
using Helpers.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.ViewModels;

namespace WebApi.Controllers.AdminDashboard
{
    [Authorize(Policy = KeyValueConstants.AdminPolicy)]
    public class UsersController : BaseMVCController
    {
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page_number,
            [FromQuery] string name,
            [FromQuery] Guid? role)
        {
            var rolesLookup = await Mediator.Send(new GetRoles.Query());

            var result = await Mediator.Send(new UsersList.Query(name, null, null, page_number, 10,
                role));

            ViewBag.roles = rolesLookup.Data;
            ViewBag.CurrentFilter = name;

            return View(new UserListVM
            {
                ListWithPagination = result,
                role = role
            });
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var rolesLookup = await Mediator.Send(new GetRoles.Query());
            ViewBag.roles = rolesLookup.Data;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserVM model)
        {
            if (!ModelState.IsValid)
            {
                var rolesLookup = await Mediator.Send(new GetRoles.Query());
                ViewBag.roles = rolesLookup.Data;

                return View(model);
            }

            var isUniqueExistResult = await Mediator.Send(new IsEmailUnique.Query(model.email));

            if (!isUniqueExistResult.IsSuccess)
            {
                var rolesLookup = await Mediator.Send(new GetRoles.Query());
                ViewBag.roles = rolesLookup.Data;

                ModelState.AddModelError(nameof(model.email), isUniqueExistResult.Message);

                return View(model);
            }

            var result = await Mediator.Send(new AddUser.Command(model.first_name, model.last_name, 
                model.email, model.role));

            TempData["isSuccess"] = result.IsSuccess;
            TempData["message"] = result.Message;

            return RedirectToAction(nameof(Add));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await Mediator.Send(new GetUserDetails.Query(id));

            if (result.Data is null)
                return RedirectToAction("NotFound", "Home");

            var model = new EditUserVM
            {
                id = id,
                first_name = result.Data.first_name,
                last_name = result.Data.last_name,
                role = result.Data.role,
                email = result.Data.email,
            };

            var rolesLookup = await Mediator.Send(new GetRoles.Query());
            ViewBag.roles = rolesLookup.Data;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserVM model)
        {
            if (!ModelState.IsValid)
            {
                var rolesLookup = await Mediator.Send(new GetRoles.Query());
                ViewBag.roles = rolesLookup.Data;

                return View(model);
            }

            var isUniqueExistResult = await Mediator.Send(new IsEmailUnique.Query(model.email, model.id));

            if (!isUniqueExistResult.IsSuccess)
            {
                var rolesLookup = await Mediator.Send(new GetRoles.Query());
                ViewBag.roles = rolesLookup.Data;

                ModelState.AddModelError(nameof(model.email), isUniqueExistResult.Message);

                return View(model);
            }

            var result = await Mediator.Send(new EditUser.Command(
                model.id, model.first_name, model.last_name, model.email, model.role));

            TempData["isSuccess"] = result.IsSuccess;
            TempData["message"] = result.Message;

            return RedirectToAction(nameof(Edit), new { id = model.id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteUser.Command(id));

            return Json(result);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var isEmailUniqueResult = await Mediator.Send(new IsEmailUnique.Query(email));

            if (isEmailUniqueResult.IsSuccess)
                return Json(true);
            else
                return Json(isEmailUniqueResult.Message);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsValidEmailDomain(string email)
        {
            var isValidDomainResult = await Mediator.Send(new IsValidEmailDomain.Query(email));

            if (isValidDomainResult.IsSuccess)
                return Json(true);
            else
                return Json(isValidDomainResult.Message);
        }
    }
}
