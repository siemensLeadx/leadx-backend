using Domain.Enums;

namespace Application.Features.Leads.Queries
{
    public partial class GetUserLeads
    {
        public class UserLeadsResponseDTO
        {
            public long lead_id { get; set; }
            public string lead_name { get; set; }
            public string hospital_name { get; set; }
            public LeadStatuses lead_status_id { get; set; }
            public string lead_status { get; set; }
            public string lead_status_note { get; set; }
            public string lead_status_back_color { get; set; }
            public string lead_status_text_color { get; set; } 
            public long created_on { get; set; }
        }
    }

    
}
