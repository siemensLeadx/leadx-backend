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
    public class IsValidEmailDomain
    {
        public class Query : IRequest<Result>
        {
            public string email { get; }
            public Query(string email)
            {
                this.email = email;
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IApplicationConfiguration _config;
            private readonly IApplicationLocalization _localizer;

            public Handler(IApplicationConfiguration config, IApplicationLocalization localizer)
            {
                _config = config;
                _localizer = localizer;
            }
            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var stringSegments = request.email.ToString().Split('@');
                var result =  stringSegments[1].Trim().ToLower() == _config.GetAppSettings().Email_Domain;

                return result ? Result.Success(string.Empty) :
                    Result.Fail(_localizer.Get(ResourceKeys.InvalidEmailDomain));
            }
        }
    }
}
