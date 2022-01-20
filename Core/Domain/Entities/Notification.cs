using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Extensions;

namespace Domain.Entities
{
    public class Notification
    {
        public long Id { get; }
        public string MessageEn { get; private set; }
        public string MessageAr { get; private set; }
        public Guid UserId { get; private set; }
        public virtual AppUser User { get; private set; }
        public long LeadId { get; private set; }
        public virtual Lead Lead { get; private set; }
        public DateTime SendOn { get; private set; }
        public LeadStatuses LeadStatusId { get; set; }
        public virtual LeadStatus LeadStatus { get; set; }

        public static Notification CreateNotification(string messageEn, string messageAr, Guid userId,
            Lead lead)
        {
            Precondition.Requires(!string.IsNullOrWhiteSpace(messageEn), "English message required",
                nameof(messageEn));

            Precondition.Requires(!string.IsNullOrWhiteSpace(messageAr), "Arabic message required",
                nameof(messageAr));

            Precondition.Requires(userId != Guid.Empty, "Invalid value for user id", nameof(userId));

            Precondition.Requires(lead != null, "Lead can not be null", nameof(lead));

            return new Notification
            {
                MessageAr = messageAr,
                MessageEn = messageEn,
                UserId = userId,
                LeadId = lead.Id,
                SendOn = new DateTime().SystemDateTimeNow(),
                LeadStatusId = lead.CurrentLeadStatusId
            };
        }
    }
}
