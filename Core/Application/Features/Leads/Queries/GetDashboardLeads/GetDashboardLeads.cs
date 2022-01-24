using Application.Interfaces;
using Application.Specifications.Leads;
using Domain.Entities;
using Domain.Enums;
using Helpers.Interfaces;
using Helpers.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Leads.Queries.GetDashboardLeads
{
    public partial class GetDashboardLeads
    {
        public class Query : IRequest<PaginatedResult<DashboardLeadsResponseDTO>>
        {
            public string name { get; }
            public LeadStatuses? status { get; }
            public Regions? region { get; }
            public Sectors? sector { get; }
            public int page_number { get; }
            public int page_size { get; }

            public Query(int page_number, int page_size, string name,
                LeadStatuses? status, Regions? region, Sectors? sector)
            {
                this.page_number = page_number;
                this.page_size = page_size;
                this.name = name;
                this.status = status;
                this.region = region;
                this.sector = sector;
            }
        }

        public class Handler : IRequestHandler<Query, PaginatedResult<DashboardLeadsResponseDTO>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow, IAuthenticatedUserService authenticatedUserService, IApplicationLocalization localizer)
            {
                _uow = uow;
                _authenticatedUserService = authenticatedUserService;
                _localizer = localizer;
            }
            public Task<PaginatedResult<DashboardLeadsResponseDTO>> Handle(Query request,
                CancellationToken cancellationToken)
            {
                var result = _uow.Repository<Lead>()
                    .PaginatedList(
                    new DashboardLeadsOrderedByCreationDateSpec(_localizer.CurrentLangWithCountry,
                        request.name, request.status, request.region, request.sector),
                    request.page_number,
                    request.page_size);

                return Task.FromResult(result);
            }
        }

        
    }
}
