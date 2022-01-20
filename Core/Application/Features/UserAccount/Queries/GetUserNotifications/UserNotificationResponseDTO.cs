using Domain.Enums;

namespace Application.Features.UserAccount.Queries
{
    public class UserNotificationResponseDTO
    {
        public long lead_id { get; set; }
        public string message { get; set; }
        public long sent_on { get; set; }
        public LeadStatuses lead_status_id { get; set; }
        public string lead_status { get; set; }
    }
}
