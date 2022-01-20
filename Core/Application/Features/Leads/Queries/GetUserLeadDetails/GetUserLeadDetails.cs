using Application.Interfaces;
using Application.Specifications.Leads;
using Domain.Entities;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Interfaces;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Leads.Queries.GetUserLeadDetails
{
    public partial class GetUserLeadDetails
    {
        public class Query : IRequest<Result<UserLeadDetailsResponseDTO>>
        {
            public long lead_id { get; }
            public Query(long lead_id)
            {
                this.lead_id = lead_id;
            }
        }

        public class Handler : IRequestHandler<Query, Result<UserLeadDetailsResponseDTO>>
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
            public async Task<Result<UserLeadDetailsResponseDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userId = Guid.Parse(_authenticatedUserService.UserId);
                var result = await _uow.Repository<Lead>()
                    .GetBySpecAsync(new UserLeadDetailsSpec(request.lead_id, userId, _localizer.CurrentLangWithCountry));

                if(result is null)
                    throw new AppCustomException(ErrorStatusCodes.NotFound,
                           new List<Tuple<string, string>> { new Tuple<string, string>(nameof(request.lead_id), ResourceKeys.DataNotFound) });


                return new Result<UserLeadDetailsResponseDTO>
                {
                    Data = result
                };
            }
        }
    }
}
