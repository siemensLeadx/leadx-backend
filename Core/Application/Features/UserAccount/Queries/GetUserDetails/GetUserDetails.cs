using Application.Interfaces;
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
    public partial class GetUserDetails
    {
        public class Query : IRequest<Result<UserDetailsResponseDTO>>
        {
            public Guid id { get; }
            public Query(Guid id)
            {
                this.id = id;
            }
        }

        public class Handler : IRequestHandler<Query, Result<UserDetailsResponseDTO>>
        {
            private readonly IUnitOfWork _uow;

            public Handler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Result<UserDetailsResponseDTO>> Handle(Query request, 
                CancellationToken cancellationToken)
            {
                var result = _uow.Repository<AppUser>()
                    .Entity()
                    .Where(user => user.Id == request.id)
                    .Select(user => new UserDetailsResponseDTO
                    {
                        first_name = user.Name.First,
                        last_name = user.Name.Last,
                        email = user.Email,
                        role = user.UserRoles.First().RoleId
                    }).FirstOrDefault();

                return new Result<UserDetailsResponseDTO>
                {
                    Data = result
                };
            }
        }
    }
}
