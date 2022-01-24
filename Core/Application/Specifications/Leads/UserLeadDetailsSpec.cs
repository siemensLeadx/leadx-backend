using Domain.Entities;
using Domain.Enums;
using Helpers.Classes;
using Helpers.Constants;
using Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Features.Leads.Queries.GetUserLeadDetails.GetUserLeadDetails;

namespace Application.Specifications.Leads
{
    public class UserLeadDetailsSpec : Specification<Lead, UserLeadDetailsResponseDTO>
    {
        public UserLeadDetailsSpec(long lead_id, Guid userId, string lang)
        {
            Query.Where(lead => lead.UserId == userId && lead.Id == lead_id);

            Query.Select(lead => new UserLeadDetailsResponseDTO
            {
                lead_id = lead.Id,
                lead_name = lead.LeadName,
                hospital_name = lead.HospitalName,
                city = lead.City,
                contact_person = lead.ContactPerson,
                comment = lead.Comment,
                customer_due_date = lead.CustomerDueDate.ToUnixTimeStamp(),
                created_on = lead.CreatedOn.ToUnixTimeStamp(),
                business_opportunity_type = lead.BusinessOpportunityType == null ? null :
                    lang.Contains(KeyValueConstants.Arabic) ? lead.BusinessOpportunityType.NameAr : lead.BusinessOpportunityType.NameEn,
                customer_status = lang.Contains(KeyValueConstants.Arabic) ? lead.CustomerStatus.NameAr : lead.CustomerStatus.NameEn,
                region = lead.RegionArea == null ? null :
                    lang.Contains(KeyValueConstants.Arabic) ? lead.RegionArea.NameAr : lead.RegionArea.NameEn,
                sector = lead.Sector == null ? null :
                    lang.Contains(KeyValueConstants.Arabic) ? lead.Sector.NameAr : lead.Sector.NameEn,
                lead_status_id = lead.CurrentLeadStatusId,
                lead_status_note = lead.StatusHistory.Where(h => h.StatusId == lead.CurrentLeadStatusId)
                    .OrderByDescending(h => h.CreatedAt).FirstOrDefault().Notes,
                lead_status = lang.Contains(KeyValueConstants.Arabic) ? lead.CurrentLeadStatus.NameAr : lead.CurrentLeadStatus.NameEn,
                lead_status_back_color = lead.CurrentLeadStatus.BackgroundColor,
                lead_status_text_color = lead.CurrentLeadStatus.TextColor,
                devices = lead.LeadNeeds.Select(d => lang.Contains(KeyValueConstants.Arabic) ? 
                    d.NeededDevice.NameAr : d.NeededDevice.NameEn).ToArray(),
                reward = GetLeadReward(lead)
            });
        }

        private static int? GetLeadReward(Lead lead)
        {
            if (lead.CurrentLeadStatusId == LeadStatuses.Approved)
            {
                return (int)lead.PromotedPrize;
            }
            else if (lead.CurrentLeadStatusId == LeadStatuses.Ordered)
            {
                return (int)lead.OrderedPrize;
            }
            else
                return null;
        }
    }
}
