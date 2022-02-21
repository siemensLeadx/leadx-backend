using Application.Interfaces;
using Application.Specifications.Leads;
using Domain.Entities;
using Domain.Enums;
using Helpers.Interfaces;
using Helpers.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Leads.Queries.GetDashboardLeadsForExcel
{
    public partial class GetDashboardLeadsForExcel
    {
        public class Query : IRequest<IEnumerable<DashboardLeadsForExcelResponseDTO>>
        {
            public string name { get; }
            public LeadStatuses? status { get; }
            public Regions? region { get; }
            public Sectors? sector { get; }
            public DateTime? from { get; }
            public DateTime? to { get; }

            public Query(string name,
                LeadStatuses? status, Regions? region, Sectors? sector, DateTime? from, DateTime? to)
            {
                this.name = name;
                this.status = status;
                this.region = region;
                this.sector = sector;
                this.from = from;
                this.to = to;
            }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<DashboardLeadsForExcelResponseDTO>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow, IApplicationLocalization localizer)
            {
                _uow = uow;
                _localizer = localizer;
            }
            public async Task<IEnumerable<DashboardLeadsForExcelResponseDTO>> Handle(Query request,
                CancellationToken cancellationToken)
            {
                var result = await _uow.Repository<Lead>()
                    .ListAsync(
                    new DashboardLeadsForExcelSpec(_localizer.CurrentLangWithCountry,
                        request.name, request.status, request.region, request.sector, request.from, request.to));

                return result;
            }
        }

        
    }
}
