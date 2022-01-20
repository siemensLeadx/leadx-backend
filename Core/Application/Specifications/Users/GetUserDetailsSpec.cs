using Application.Features.UserAccount.Queries;
using Domain.Entities;
using Helpers.Classes;
using Helpers.Constants;
using Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Users
{
    public class GetUserDetailsSpec : Specification<AppUser>
    {
        public GetUserDetailsSpec(Guid userId)
        {
            Query.Where(user => user.Id == userId)
                .Include(user => user.UserRoles);
        }
    }
}
