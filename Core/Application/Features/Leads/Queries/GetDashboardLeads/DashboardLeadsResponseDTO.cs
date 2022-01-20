using System;

namespace Application.Features.Leads.Queries.GetDashboardLeads
{
    public class DashboardLeadsResponseDTO
    {
        public long lead_id { get; set; }
        public string lead_name { get; set; }
        public string hospital_name { get; set; }
        public string lead_status { get; set; }
        public string lead_status_back_color { get; set; }
        public string lead_status_text_color { get; set; }
        public long created_on { get; set; }
        public string created_by { get; set; }
    }
}
