using Application.Authorization;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using FluentValidation;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Interfaces;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Commands
{
    public class Login
    {
        public class Command : IRequest<Result<LoginResponseDTO>>
        {
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            //public string login_provider { get; set; }
            public string login_provider_id { get; set; }

            [JsonIgnore]
            public string ip_address { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer,
                IApplicationConfiguration applicationConfiguration)
            {
                RuleFor(p => p.email).Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .EmailAddress()
                    .Must(p => {
                        var domain = p.Trim().ToLower().Split("@")[1];
                        return applicationConfiguration.GetAppSettings().Email_Domain.Contains(domain);
                    })
                    .WithMessage(p => localizer.Get(ResourceKeys.InvalidEmailDomain))
                    .WithName(p => localizer.Get(ResourceKeys.Email));

                RuleFor(p => p.first_name).NotEmpty()
                   .MaximumLength(100)
                   .WithName(x => localizer.Get(ResourceKeys.FirstName));

                RuleFor(p => p.last_name).NotEmpty()
                    .MaximumLength(100).WithName(localizer.Get(ResourceKeys.LastName));

                RuleFor(p => p.login_provider_id).NotEmpty()
                    .WithName(localizer.Get(ResourceKeys.LoginProviderId));
            }
        }

        public class Handler : IRequestHandler<Command, Result<LoginResponseDTO>>
        {
            private readonly IIdentityService _identityService;
            private readonly IApplicationConfiguration _configuration;
            private readonly IUnitOfWork _uow;

            public Handler(IIdentityService identityService, IApplicationConfiguration configuration, IUnitOfWork uow)
            {
                _identityService = identityService;
                _configuration = configuration;
                _uow = uow;
            }
            public async Task<Result<LoginResponseDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindByName(request.email);

                if (user == null)
                {
                    var loginProviderExist = _uow.Repository<AppUserLogin>()
                        .Entity()
                        .Any(e => e.LoginProvider == KeyValueConstants.MicrosoftLoginProviderName && e.ProviderKey == request.login_provider_id);

                    if(loginProviderExist)
                        throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                            new List<Tuple<string, string>> { new Tuple<string, string>(nameof(request.login_provider_id), ResourceKeys.DuplicateProviderId) });

                    var name = Name.Create(request.first_name, request.last_name);

                    user = new AppUser(name, request.email, request.email);

                    user.Logins.Add(new AppUserLogin
                    {
                        LoginProvider = KeyValueConstants.MicrosoftLoginProviderName,
                        ProviderDisplayName = KeyValueConstants.MicrosoftLoginProviderName,
                        ProviderKey = request.login_provider_id
                    });

                    await _identityService.Add(user);
                    await _identityService.AddUserToRole(user, DefaultRoles.USER.ToString());
                }
                else
                {
                    var userHasProviderId = user.Logins.Any();

                    if (!userHasProviderId)
                    {
                        user.Logins.Add(new AppUserLogin
                        {
                            LoginProvider = KeyValueConstants.MicrosoftLoginProviderName,
                            ProviderDisplayName = KeyValueConstants.MicrosoftLoginProviderName,
                            ProviderKey = request.login_provider_id
                        });

                        await _uow.CompleteAsync();
                    }

                    if (!(await _identityService.IsUserInRole(user, DefaultRoles.USER.ToString())) ||
                       !user.Logins.Any(a => a.ProviderKey == request.login_provider_id))
                        throw new AppCustomException(ErrorStatusCodes.UnAuthorized,
                            new List<Tuple<string, string>> { new Tuple<string, string>(nameof(request.email), ResourceKeys.NotAllowedUser) });
                }           

                var accessToken = await _identityService.GenerateAccessToken(user, request.ip_address);
                var refreshToken = _identityService.GenerateRefreshToken();

                return new Result<LoginResponseDTO>
                {
                    Data = new LoginResponseDTO
                    {
                        user = new LoginUserDataResponseDTO
                        {
                            id = user.Id.ToString(),
                            name = $"{user.Name.First} {user.Name.Last}",
                            email = user.Email
                        },
                        access_token = accessToken,
                        refresh_token = refreshToken,
                        token_type = KeyValueConstants.TokenType,
                        expires_in = _configuration.GetJwtSettings().DurationInMillisecond
                    }
                };
            }
        }
    }
}
