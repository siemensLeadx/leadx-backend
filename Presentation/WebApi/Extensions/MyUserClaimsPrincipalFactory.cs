using Domain.Entities;
using Helpers.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser>
    {
        public MyUserClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            foreach (var role in user.UserRoles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Role.Alias));
                identity.AddClaim(new Claim(ClaimTypes.GivenName, $"{user.Name.First} {user.Name.Last}"));
            }
            
            return identity;
        }
    }
}
