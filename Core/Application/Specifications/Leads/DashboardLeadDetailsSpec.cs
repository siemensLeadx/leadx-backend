using Application.Features.Leads.Queries.GetDashboardLeadDetails;
using Domain.Entities;
using Helpers.Classes;
using Helpers.Constants;
using Helpers.Extensions;
using System.Linq;

namespace Application.Specifications.Leads
{
    public class DashboardLeadDetailsSpec : Specification<Lead, DashboardLeadDetailsResponseDTO>
    {
        public DashboardLeadDetailsSpec(long lead_id, string lang)
        {
            Query.Where(lead => lead.Id == lead_id);
            Query.Include(lead => lead.BusinessOpportunityType);
            Query.Include(lead => lead.RewardClass);
            Query.Include(lead => lead.RewardCriteria);

            Query.Select(lead => new DashboardLeadDetailsResponseDTO
            {
                lead_id = lead.Id,
                lead_name = lead.LeadName,
                hospital_name = lead.HospitalName,
                region = lead.Region,
                contact_person = lead.ContactPerson,
                comment = lead.Comment,
                customer_due_date = lead.CustomerDueDate.ToUnixTimeStamp(),
                created_on = lead.CreatedOn.ToUnixTimeStamp(),
                business_opportunity_type = lead.BusinessOpportunityType == null ? null : SetOpportunityTypeName(lead, lang),
                customer_status = lang.Contains(KeyValueConstants.Arabic) ? lead.CustomerStatus.NameAr : lead.CustomerStatus.NameEn,
                lead_status = lang.Contains(KeyValueConstants.Arabic) ? lead.CurrentLeadStatus.NameAr : lead.CurrentLeadStatus.NameEn,
                lead_status_back_color = lead.CurrentLeadStatus.BackgroundColor,
                lead_status_text_color = lead.CurrentLeadStatus.TextColor,
                lead_status_id = lead.CurrentLeadStatusId,
                lead_status_notes = lead.StatusHistory.Where(h => h.StatusId == lead.CurrentLeadStatusId)
                    .OrderByDescending(h => h.CreatedAt).FirstOrDefault().Notes,
                devices = lead.LeadNeeds.Select(d => lang.Contains(KeyValueConstants.Arabic) ? 
                    d.NeededDevice.NameAr : d.NeededDevice.NameEn).ToArray(),
                created_by = $"{lead.User.Name.First} {lead.User.Name.Last}",
                reward_class_id = lead.RewardClassId,
                reward_class = lead.RewardClassId.HasValue ? SetRewardClassName(lead, lang) : null,
                reward_criteria_id = lead.RewardCriteriaId,
                reward_criteria = lead.RewardCriteriaId.HasValue ? lead.RewardCriteria.ToCustomStringFormat(lang) : null
            });
        }

        private static string SetRewardClassName(Lead lead, string lang)
        {
            if (lang.Contains(KeyValueConstants.Arabic))
                return lead.RewardClass.NameAr;
            else
                return lead.RewardClass.NameEn;
        }

        private static string SetOpportunityTypeName(Lead lead, string lang)
        {
            if (lang.Contains(KeyValueConstants.Arabic))
                return lead.BusinessOpportunityType.NameAr;
            else
                return lead.BusinessOpportunityType.NameEn;
        }
    }
}
