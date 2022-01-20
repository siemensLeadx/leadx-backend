using Domain.Entities;
using Helpers.Classes;
using Helpers.Constants;
using Helpers.Extensions;
using System;
using System.Linq;
using static Application.Features.Leads.Queries.GetUserLeads;

namespace Application.Specifications.Leads
{
    public class UserLeadsOrderedByCreationDateSpec : Specification<Lead, UserLeadsResponseDTO>
    {
        public UserLeadsOrderedByCreationDateSpec(Guid userId, string lang, string name)
        {
            Query.Where(lead => lead.UserId == userId && 
                        (string.IsNullOrWhiteSpace(name) || lead.LeadName.Contains(name)))
                .OrderByDescending(lead => lead.CreatedOn);

            Query.Include(lead => lead.StatusHistory);

            Query.Select(lead => new UserLeadsResponseDTO
            {
                lead_id = lead.Id,
                lead_name = lead.LeadName,
                hospital_name = lead.HospitalName,
                created_on = lead.CreatedOn.ToUnixTimeStamp(),
                lead_status_id = lead.CurrentLeadStatusId,
                lead_status_note = lead.StatusHistory.Where(h => h.StatusId == lead.CurrentLeadStatusId)
                    .OrderByDescending(h => h.CreatedAt).FirstOrDefault().Notes,
                lead_status = lang.Contains(KeyValueConstants.Arabic) ? lead.CurrentLeadStatus.NameAr : lead.CurrentLeadStatus.NameEn,
                lead_status_back_color = lead.CurrentLeadStatus.BackgroundColor,
                lead_status_text_color = lead.CurrentLeadStatus.TextColor,
            });
        }
    }
}
