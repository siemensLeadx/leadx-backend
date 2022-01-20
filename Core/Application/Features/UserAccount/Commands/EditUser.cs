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
    public class EditUser
    {
        public class Command: IRequest<Result>
        {
            public Guid id { get; }
            public string first_name { get; }
            public string last_name { get; }
            public string email { get; }
            public Guid role_id { get; set; }

            public Command(Guid id, string first_name, string last_name, string email, Guid role_id)
            {
                this.id = id;
                this.first_name = first_name;
                this.last_name = last_name;
                this.email = email;
                this.role_id = role_id;
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

                var name = Name.Create(request.first_name, request.last_name);

                user.EditData(name, request.email, null, null);
                _uow.Repository<AppUserRole>().Remove(user.UserRoles.First());
                user.UserRoles.Add(new AppUserRole { RoleId = request.role_id });

                await _uow.CompleteAsync();

                return Result.Success(_localizer.Get(ResourceKeys.SavedSuccessfully));
            }
        }
    }
}
