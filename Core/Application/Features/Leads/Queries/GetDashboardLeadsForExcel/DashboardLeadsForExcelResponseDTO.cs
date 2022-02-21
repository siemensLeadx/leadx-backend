using System;

namespace Application.Features.Leads.Queries.GetDashboardLeadsForExcel
{
    public class DashboardLeadsForExcelResponseDTO
    {
        public long lead_id { get; set; }
        public string lead_name { get; set; }
        public string hospital { get; set; }
        public string status { get; set; }
        public string created_on { get; set; }
    }
}
