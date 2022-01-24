using Application.Features.Leads.Queries.GetDashboardLeads;
using Domain.Enums;
using Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class LeadsListVM
    {
        public LeadStatuses? Status { get; set; }
        public Regions? Region { get; set; }
        public Sectors? Sector { get; set; }
        public PaginatedResult<DashboardLeadsResponseDTO> ListWithPagination { get; set; }
    }
}
