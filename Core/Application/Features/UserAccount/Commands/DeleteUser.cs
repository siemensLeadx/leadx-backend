using Application.Interfaces;
using Application.Specifications.Users;
using Domain.Entities;
using Domain.ValueObjects;
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

namespace Application.Features.UserAccount.Commands
{
    public class DeleteUser
    {
        public class Command: IRequest<Result>
        {
            public Guid id { get; }

            public Command(Guid id)
            {
                this.id = id;
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IUnitOfWork _uow;
            private readonly IApplicationLocalization _localizer;

            public Handler(IUnitOfWork uow, IApplicationLocalization localizer)
            {
                _uow = uow;
                _localizer = localizer;
            }
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _uow.Repository<AppUser>()
                    .GetBySpecAsync(new GetUserDetailsSpec(request.id));

                if (user == null)
                    return Result.Fail(_localizer.Get(ResourceKeys.UserNotFound));

                user.IsDeleted = true;
                _uow.Repository<AppUserRole>().RemoveRange(user.UserRoles);
                _uow.Repository<AppUser>().Remove(user);

                await _uow.CompleteAsync();

                return Result.Success(_localizer.Get(ResourceKeys.DeletedSuccessfully));
            }
        }
    }
}
