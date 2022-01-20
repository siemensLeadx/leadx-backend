using Application.Features.UserAccount.Queries;
using Domain.Entities;
using Helpers.Classes;
using Helpers.Constants;
using Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Users
{
    public class UserNotificationsOrderedBySentDateSpec : Specification<Notification, UserNotificationResponseDTO>
    {
        public UserNotificationsOrderedBySentDateSpec(Guid userId, string lang)
        {
            Query.Where(not => not.UserId == userId)
                 .OrderByDescending(not => not.SendOn);

            Query.Include(not => not.Lead);
            Query.Include(not => not.LeadStatus);

            Query.Select(not => new UserNotificationResponseDTO
            {
                lead_id = not.Lead.Id,
                lead_status = lang.Contains(KeyValueConstants.Arabic) ? not.LeadStatus.NameAr : not.LeadStatus.NameEn,
                lead_status_id = not.LeadStatusId,
                message = lang.Contains(KeyValueConstants.Arabic) ? not.MessageAr : not.MessageEn,
                sent_on = not.SendOn.ToUnixTimeStamp()
            });
        }
    }
}
