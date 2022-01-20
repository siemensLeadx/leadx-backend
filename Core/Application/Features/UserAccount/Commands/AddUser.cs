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
    public class AddUser
    {
        public class Command: IRequest<Result>
        {
            public string first_name { get; }
            public string last_name { get; }
            public string email { get; }
            public Guid role_id { get; set; }

            public Command(string first_name, string last_name, string email, Guid role_id)
            {
                this.first_name = first_name;
                this.last_name = last_name;
                this.email = email;
                this.role_id = role_id;
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IIdentityService _identityService;
            private readonly IApplicationLocalization _localizer;

            public Handler(IIdentityService identityService, IApplicationLocalization localizer)
            {
                _identityService = identityService;
                _localizer = localizer;
            }
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var name = Name.Create(request.first_name, request.last_name);

                var newUser = new AppUser(name, request.email.Trim(), request.email.Trim());
                newUser.UserRoles.Add(new AppUserRole { RoleId = request.role_id });
                await _identityService.Add(newUser);

                return Result.Success(_localizer.Get(ResourceKeys.SavedSuccessfully));
            }
        }
    }
}
