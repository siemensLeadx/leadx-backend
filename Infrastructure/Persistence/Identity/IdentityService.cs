using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Helpers.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        protected readonly ApplicationDbContext _dbContext;
        private readonly IApplicationConfiguration _applicationConfiguration;

        public IdentityService(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext dbContext,
            IApplicationConfiguration applicationConfiguration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _applicationConfiguration = applicationConfiguration;
            _roleManager = roleManager;
        }
        public IQueryable<AppUser> Entities => _dbContext.Users;
        public async Task<CreateUserResponseDTO> Add(AppUser user, string password = null)
        {
            var identityResult = string.IsNullOrWhiteSpace(password) ? await _userManager.CreateAsync(user) 
                : await  _userManager.CreateAsync(user, password);

            string verificationToken = null;

            if (identityResult.Succeeded)
                verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return new CreateUserResponseDTO
            {
                success = identityResult.Succeeded,
                errors = identityResult.Succeeded ? null : identityResult.Errors
                        .Select(a => new Tuple<string, string> (a.Code, a.Description)).ToList(),
                verification_token = verificationToken
            };
        }

        public async Task<IdentityResponseDTO> Update(AppUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            return new IdentityResponseDTO
            {
                success = result.Succeeded,
                errors = result.Succeeded ? null : result.Errors
                        .Select(a => new Tuple<string, string>(a.Code, a.Description)).ToList(),
            };
        }

        public async Task<AppUser> FindByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<AppUser> FindById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<AppUser> FindByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }   

        public async Task<bool> CheckPassword(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        
        public async Task<IdentitySignInResponseDTO> Login(AppUser user, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, false, true);

            return new IdentitySignInResponseDTO
            {
                success = result.Succeeded,
                isLockedOut = result.IsLockedOut
            };
        }
        
        public async Task<IdentityResponseDTO> VerifyUserAccount(AppUser user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return new IdentityResponseDTO
            {
                success = result.Succeeded,
                errors = result.Succeeded ? null : result.Errors
                        .Select(a => new Tuple<string, string>(a.Code, a.Description)).ToList(),
            };
        }

        public async Task<string> GenerateForgetPasswordToken(AppUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResponseDTO> ResetPassword(AppUser user, string token, string password)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            return new IdentityResponseDTO
            {
                success = result.Succeeded,
                errors = result.Succeeded ? null : result.Errors
                        .Select(a => new Tuple<string, string>(a.Code, a.Description)).ToList(),
            };
        }

        public async Task<string> GenerateAccessToken(AppUser user, string ipAddress)
        {
            var jwtSettings = _applicationConfiguration.GetJwtSettings();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretHashKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            List<Claim> userClaims = new List<Claim>();

            var claims = await _userManager.GetClaimsAsync(user);
            userClaims.AddRange(claims);

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                var r = await _roleManager.FindByNameAsync(role);
                var roleClaims = await _roleManager.GetClaimsAsync(r);
                userClaims.AddRange(roleClaims);
            }

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            userClaims.Add(new Claim(KeyValueConstants.UsernameClaimType, user.UserName));
            if(!string.IsNullOrWhiteSpace(ipAddress))
                userClaims.Add(new Claim(KeyValueConstants.IP, ipAddress));

            foreach (var role in roles)
            {
                var roleValue = await _roleManager.FindByNameAsync(role);
                userClaims.Add(new Claim(KeyValueConstants.Role, roleValue.Alias));
            }

            var token = new JwtSecurityToken
            (
                KeyValueConstants.Issuer,
                KeyValueConstants.Audience,
                userClaims.GroupBy(x => x.Value).Select(y => y.First()).Distinct(),
                DateTime.Now,
                DateTime.Now.AddMilliseconds(jwtSettings.DurationInMillisecond),
                credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public async Task<bool> IsUserInRole(AppUser user, string roleName)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                var roleValue = await _roleManager.FindByNameAsync(role);

                if (roleValue.Alias == roleName) return true;
            }

            return false;
        }

        public async Task<bool> IsUserInRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                var roleValue = await _roleManager.FindByNameAsync(role);

                if (roleValue.Alias == roleName) return true;
            }

            return false;
        }

        public async Task AddUserToRole(AppUser user, string roleName)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(role => role.Alias == roleName);

            user.UserRoles.Add(new AppUserRole { RoleId = role.Id });

            await _dbContext.SaveChangesAsync();
        }
    }
}
