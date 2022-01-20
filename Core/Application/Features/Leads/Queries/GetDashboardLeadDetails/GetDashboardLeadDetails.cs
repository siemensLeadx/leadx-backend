using Application.Interfaces;
using Application.Specifications.Leads;
using Domain.Entities;
using Helpers.Interfaces;
using Helpers.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Leads.Queries.GetDashboardLeadDetails
{
    public partial class GetDashboardLeadDetails
    {
        public class Query : IRequest<Result<DashboardLeadDetailsResponseDTO>>
        {
            public long lead_id { get; }
            public Query(long lead_id)
            {
                this.lead_id = lead_id;
            }
        }

        public class Handler : IRequestHandler<Query, Result<DashboardLeadDetailsResponseDTO>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow, IApplicationLocalization localizer)
            {
                _uow = uow;
                _localizer = localizer;
            }
            public async Task<Result<DashboardLeadDetailsResponseDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _uow.Repository<Lead>()
                    .GetBySpecAsync(new DashboardLeadDetailsSpec(request.lead_id, 
                        _localizer.CurrentLangWithCountry));

                return new Result<DashboardLeadDetailsResponseDTO>
                {
                    Data = result
                };
            }
        }
    }
}
