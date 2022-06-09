using Application.Features.Leads.Queries.GetDashboardLeadsForExcel;
using Domain.Entities;
using Domain.Enums;
using Helpers.Classes;
using Helpers.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Leads
{
    public class DashboardLeadsForExcelSpec : Specification<Lead, DashboardLeadsForExcelResponseDTO>
    {
        public DashboardLeadsForExcelSpec(string lang, string name,
            LeadStatuses? status, Regions? region, Sectors? sector, DateTime? from, DateTime? to)
        {
            Query.Where(lead => (string.IsNullOrWhiteSpace(name) ||
                                 lead.LeadName.Contains(name) ||
                                 lead.User.Email.Contains(name)) &&
                                (!status.HasValue || lead.CurrentLeadStatusId == status.Value) &&
                                (!region.HasValue || lead.RegionId == region.Value) &&
                                (!sector.HasValue || lead.SectorId == sector.Value) &&
                                (!from.HasValue || lead.CreatedOn.Date >= from.Value.Date) &&
                                (!to.HasValue || lead.CreatedOn.Date <= to.Value.Date))
                .OrderByDescending(lead => lead.CreatedOn);

            Query.Select(lead => new DashboardLeadsForExcelResponseDTO
            {
                lead_id = lead.Id,
                lead_name = lead.LeadName,
                hospital = lead.HospitalName,
                created_on = lead.CreatedOn.ToString("dd/MM/yyyy"),
                status = lang.Contains(KeyValueConstants.Arabic) ? lead.CurrentLeadStatus.NameAr : lead.CurrentLeadStatus.NameEn,
                created_by = $"{lead.User.Name.First} {lead.User.Name.Last}  ({lead.User.Email})",
                sector = lang.Contains(KeyValueConstants.Arabic) ? lead.Sector.NameAr : lead.Sector.NameEn,
                region = lang.Contains(KeyValueConstants.Arabic) ? lead.RegionArea.NameAr : lead.RegionArea.NameEn
            });
        }
    }
}
