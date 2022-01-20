using Application.Interfaces;
using Helpers.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) == null ? null :
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
        public string Username =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(KeyValueConstants.UsernameClaimType) == null ?
                (_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name) == null ? null : _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value) :
            _httpContextAccessor.HttpContext?.User?.FindFirst(KeyValueConstants.UsernameClaimType).Value;
    }
}
