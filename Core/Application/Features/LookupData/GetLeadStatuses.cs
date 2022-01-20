using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Entities;
using Helpers.Constants;
using Helpers.Interfaces;
using Helpers.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.LookupData
{
    public class GetLeadStatuses
    {
        public class Query : IRequest<Result<IEnumerable<LookupResponseDTO>>>
        {

        }

        public class Handler : IRequestHandler<Query, Result<IEnumerable<LookupResponseDTO>>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow, IApplicationLocalization localizer)
            {
                _uow = uow;
                _localizer = localizer;
            }
            public async Task<Result<IEnumerable<LookupResponseDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = (await _uow.Repository<LeadStatus>()
                    .ListAsync()).Select(b => new LookupResponseDTO
                    {
                        id = (int)b.Id,
                        name = _localizer.CurrentLangWithCountry.Contains(KeyValueConstants.Arabic) ? b.NameAr : b.NameEn
                    });

                return new Result<IEnumerable<LookupResponseDTO>>
                {
                    Data = result
                };
            }
        }
    }
}
