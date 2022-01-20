using Application.Interfaces;
using Application.Specifications.Users;
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

namespace Application.Features.UserAccount.Queries
{
    public partial class GetUserNotifications
    {
        public class Query: IRequest<PaginatedResult<UserNotificationResponseDTO>>
        {
            public int page_number { get; }
            public int page_size { get; }

            public Query(int page_number, int page_size)
            {
                this.page_number = page_number;
                this.page_size = page_size;
            }
        }

        public class Handler : IRequestHandler<Query, PaginatedResult<UserNotificationResponseDTO>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow, IAuthenticatedUserService authenticatedUserService, 
                IApplicationLocalization localizer)
            {
                _uow = uow;
                _authenticatedUserService = authenticatedUserService;
                _localizer = localizer;
            }
            public Task<PaginatedResult<UserNotificationResponseDTO>> Handle(Query request, 
                CancellationToken cancellationToken)
            {
                var userId = Guid.Parse(_authenticatedUserService.UserId);

                var result = _uow.Repository<Notification>()
                    .PaginatedList(new UserNotificationsOrderedBySentDateSpec(userId, _localizer.CurrentLangWithCountry), 
                        request.page_number, request.page_size);

                return Task.FromResult(result);
            }
        }
    }
}
