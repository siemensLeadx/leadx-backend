using Application.Authorization;
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
    public class GetRoles
    {
        public class Query : IRequest<Result<IEnumerable<LookupWithGuidResponseDTO>>>
        {

        }

        public class Handler : IRequestHandler<Query, Result<IEnumerable<LookupWithGuidResponseDTO>>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow, IApplicationLocalization localizer)
            {
                _uow = uow;
                _localizer = localizer;
            }
            public async Task<Result<IEnumerable<LookupWithGuidResponseDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = (await _uow.Repository<AppRole>()
                    .ListAsync())
                    .Select(b => new LookupWithGuidResponseDTO
                    {
                        id = b.Id,
                        name = _localizer.CurrentLangWithCountry.Contains(KeyValueConstants.Arabic) ? b.NameAr : b.Name
                    });

                return new Result<IEnumerable<LookupWithGuidResponseDTO>>
                {
                    Data = result
                };
            }
        }
    }
}
