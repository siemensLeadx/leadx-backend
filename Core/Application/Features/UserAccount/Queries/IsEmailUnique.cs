using Application.Interfaces;
using Domain.Entities;
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

namespace Application.Features.UserAccount.Queries
{
    public class IsEmailUnique
    {
        public class Query : IRequest<Result>
        {
            public Guid? user_id { get; }
            public string email { get; }
            public Query(string email, Guid? user_id = null)
            {
                this.user_id = user_id;
                this.email = email;
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IUnitOfWork _uow;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow, IApplicationLocalization localizer)
            {
                _uow = uow;
                _localizer = localizer;
            }
            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                bool result;

                if (request.user_id.HasValue)
                    result =  _uow.Repository<AppUser>()
                                  .Entity()
                                  .Any(user => user.Email.ToLower() == request.email.Trim().ToLower() &&
                                               user.Id != request.user_id.Value); 
                else
                    result = _uow.Repository<AppUser>()
                                 .Entity()
                                 .Any(user => user.Email.ToLower() == request.email.Trim().ToLower());

                return result ? Result.Fail(_localizer.Get(ResourceKeys.DuplicateEmail))
                        : Result.Success(string.Empty);
            }
        }
    }
}
