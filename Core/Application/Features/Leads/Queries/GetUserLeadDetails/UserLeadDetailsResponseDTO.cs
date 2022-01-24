using Domain.Enums;
using System.Collections.Generic;

namespace Application.Features.Leads.Queries.GetUserLeadDetails
{
    public partial class GetUserLeadDetails
    {
        public class UserLeadDetailsResponseDTO
        {
            public long lead_id { get; set; }
            public string lead_name { get; set; }
            public string hospital_name { get; set; }
            public string city { get; set; }
            public string region { get; set; }
            public string sector { get; set; }
            public string business_opportunity_type { get; set; }
            public string customer_status { get; set; }
            public long customer_due_date { get; set; }
            public string comment { get; set; }
            public string contact_person { get; set; }
            public LeadStatuses lead_status_id { get; set; }
            public string lead_status { get; set; }
            public string lead_status_note { get; set; }
            public string lead_status_back_color { get; set; }
            public string lead_status_text_color { get; set; }
            public long created_on { get; set; }
            public string[] devices { get; set; }
            public int? reward { get; set; }
        }
    }
}
