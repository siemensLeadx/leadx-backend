using Application.Features.Leads.Commands;
using Application.Features.Leads.Queries.GetDashboardLeadDetails;
using Application.Features.Leads.Queries.GetDashboardLeads;
using Application.Features.LookupData;
using Application.Interfaces;
using Domain.Enums;
using Helpers.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApi.Services;
using WebApi.ViewModels;

namespace WebApi.Controllers.AdminDashboard
{   
    [Authorize(Policy = KeyValueConstants.HrPolicy)]
    public class DashboardLeadsController : BaseMVCController
    {
        private readonly IExcelOperations _excelOperations;
        private readonly IWebHostEnvironment _env;

        public DashboardLeadsController(IExcelOperations excelOperations,
            IWebHostEnvironment env )
        {
            _excelOperations = excelOperations;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page_number,
            [FromQuery] string name,
            [FromQuery] LeadStatuses? status,
            [FromQuery] Regions? region,
            [FromQuery] Sectors? sector,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var statusLookup = await Mediator.Send(new GetLeadStatuses.Query());
            var regionLookup = await Mediator.Send(new GetRegions.Query());
            var sectorLookup = await Mediator.Send(new GetSectors.Query());

            var result = await Mediator.Send(new GetDashboardLeads.Query(page_number, 10, name,
                status, region, sector, from, to));

            ViewBag.CurrentFilter = name;
            ViewBag.statuses = statusLookup.Data;
            ViewBag.regions = regionLookup.Data;
            ViewBag.sectors = sectorLookup.Data;

            var model = new LeadsListVM
            {
                ListWithPagination = result,
                Status = status,
                Region = region,
                Sector = sector,
                From = from,
                To = to
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ListToExcel(
            [FromQuery] string name,
            [FromQuery] LeadStatuses? status,
            [FromQuery] Regions? region,
            [FromQuery] Sectors? sector,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var result = await Mediator.Send(new Application.Features.Leads.Queries.GetDashboardLeadsForExcel.GetDashboardLeadsForExcel.Query(name,
                status, region, sector, from, to));

            var leadsExcelFile = Path.Combine(_env.WebRootPath, $"leads_{DateTime.Now.ToString("ddMMyyyyhhmmssfff")}.xlsx");

            await _excelOperations.ExportExcel(result, leadsExcelFile, "leads");

            using (MemoryStream stream = new MemoryStream())
            {
                using(FileStream fs = new FileStream(leadsExcelFile, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(stream);
                }

                System.IO.File.Delete(leadsExcelFile);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "leads.xlsx");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var result = await Mediator.Send(new GetDashboardLeadDetails.Query(id));

            if (result.Data is null)
                return RedirectToAction("NotFound", "Home");

            var rewardClassLookup = await Mediator.Send(new GetRewardClasses.Query());
            var rewardCriteriaLookup = await Mediator.Send(new GetRewardCriterias.Query());
            var leadStatusLookup = await Mediator.Send(new GetLeadStatuses.Query());

            ViewBag.rewardClasses = rewardClassLookup.Data;
            ViewBag.rewardCriterias = rewardCriteriaLookup.Data;
            ViewBag.leadStatuses = leadStatusLookup.Data;

            return View(result);
        }

        [Authorize(Policy = KeyValueConstants.AdminPolicy)]
        [HttpGet]
        public async Task<IActionResult> SetRewardCriteria(int id)
        {
            var lead = await Mediator.Send(new GetDashboardLeadDetails.Query(id));

            if (lead.Data is null)
                return RedirectToAction("NotFound", "Home");

            var rewardClassLookup = await Mediator.Send(new GetRewardClasses.Query());
            var rewardCriteriaLookup = await Mediator.Send(new GetRewardCriterias.Query());
            

            var model = new SetLeadRewardVM
            {
                lead_id = id,
                reward_class_id = lead.Data.reward_class_id,
                reward_criteria_id = lead.Data.reward_criteria_id
            };

            ViewBag.rewardClasses = rewardClassLookup.Data;
            ViewBag.rewardCriterias = rewardCriteriaLookup.Data;
            ViewBag.header = $"{lead.Data.lead_name} [#{lead.Data.lead_id}]";

            return View(model);
        }

        [Authorize(Policy = KeyValueConstants.AdminPolicy)]
        [HttpPost]
        public async Task<IActionResult> SetRewardCriteria(SetLeadRewardVM model)
        {
            var result = await Mediator.Send(new SetLeadReward.Command(model.lead_id,
                model.reward_class_id, model.reward_criteria_id));

            return Json(result);
        }

        [Authorize(Policy = KeyValueConstants.AdminPolicy)]
        [HttpPost]
        public async Task<IActionResult> AddAdminNotes(AddAdminNotesVM model)
        {
            var result = await Mediator.Send(new AddAdminNotes.Command(model.lead_id,
                model.admin_notes));

            return Json(result);
        }

        [Authorize(Policy = KeyValueConstants.AdminPolicy)]
        [HttpGet]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var lead = await Mediator.Send(new GetDashboardLeadDetails.Query(id));

            if (lead.Data is null)
                return RedirectToAction("NotFound", "Home");

            var leadStatusLookup = await Mediator.Send(new GetLeadStatuses.Query());

            var model = new ChangeLeadStatusVM
            {
                lead_id = id,
                status_id = (int)lead.Data.lead_status_id
            };

            ViewBag.leadStatuses = leadStatusLookup.Data;
            ViewBag.header = $"{lead.Data.lead_name} [#{lead.Data.lead_id}]";
            return View(model);
        }

        [Authorize(Policy = KeyValueConstants.AdminPolicy)]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(ChangeLeadStatusVM model)
        {
            var result = await Mediator.Send(new ChangeLeadStatus.Command(model.lead_id,
                (LeadStatuses)model.status_id, model.notes));

            return Json(result);
        }
    }
}
