using Application.Interfaces;
using Application.Specifications.Leads;
using Domain.Entities;
using Helpers.Interfaces;
using Helpers.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Leads.Queries
{
    public partial class GetUserLeads
    {
        public class Query : IRequest<PaginatedResult<UserLeadsResponseDTO>>
        {
            public string name { get; }
            public int page_number { get; }
            public int page_size { get; }

            public Query(int  page_number, int page_size, string name)
            {
                this.page_number = page_number;
                this.page_size = page_size;
                this.name = name;
            }
        }

        public class Handler : IRequestHandler<Query, PaginatedResult<UserLeadsResponseDTO>>
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
            public Task<PaginatedResult<UserLeadsResponseDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userId = Guid.Parse(_authenticatedUserService.UserId);
                var result = _uow.Repository<Lead>()
                    .PaginatedList(new UserLeadsOrderedByCreationDateSpec(userId, _localizer.CurrentLangWithCountry, request.name), request.page_number, request.page_size);

                return Task.FromResult(result);              
            }
        }

        
    }

    
}
