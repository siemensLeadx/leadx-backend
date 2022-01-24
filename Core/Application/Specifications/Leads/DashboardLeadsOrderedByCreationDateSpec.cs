using Application.Features.Leads.Queries.GetDashboardLeads;
using Domain.Entities;
using Domain.Enums;
using Helpers.Classes;
using Helpers.Constants;
using Helpers.Extensions;
using System.Linq;

namespace Application.Specifications.Leads
{
    public class DashboardLeadsOrderedByCreationDateSpec : Specification<Lead, DashboardLeadsResponseDTO>
    {
        public DashboardLeadsOrderedByCreationDateSpec(string lang, string name,
            LeadStatuses? status, Regions? region, Sectors? sector)
        {
            Query.Where(lead => (string.IsNullOrWhiteSpace(name) || 
                                 lead.LeadName.Contains(name) || 
                                 lead.User.Email.Contains(name)) &&
                                (!status.HasValue || lead.CurrentLeadStatusId == status.Value) &&
                                (!region.HasValue || lead.RegionId == region.Value) &&
                                (!sector.HasValue || lead.SectorId == sector.Value))
                .OrderByDescending(lead => lead.CreatedOn);

            Query.Select(lead => new DashboardLeadsResponseDTO
            {
                lead_id = lead.Id,
                lead_name = lead.LeadName,
                hospital_name = lead.HospitalName,
                created_on = lead.CreatedOn.ToUnixTimeStamp(),
                lead_status = lang.Contains(KeyValueConstants.Arabic) ? lead.CurrentLeadStatus.NameAr : lead.CurrentLeadStatus.NameEn,
                lead_status_back_color = lead.CurrentLeadStatus.BackgroundColor,
                lead_status_text_color = lead.CurrentLeadStatus.TextColor,
                created_by = $"{lead.User.Name.First} {lead.User.Name.Last} "
            });
        }
    }
}
