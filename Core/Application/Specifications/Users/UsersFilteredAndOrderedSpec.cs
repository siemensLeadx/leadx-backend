using Application.Authorization;
using Application.DTOs;
using Domain.Entities;
using Helpers.Classes;
using Helpers.Constants;
using Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Users
{
    public class UsersFilteredAndOrderedSpec : Specification<AppUser, UsersListResponseDTO>
    {
        public UsersFilteredAndOrderedSpec(string name, string sortBy, string sortOrder,
            string lang, Guid? role)
        {
            Query.Where(user => (string.IsNullOrWhiteSpace(name) ||
                                user.Name.First.ToLower().Contains(name.ToLower()) ||
                                user.Name.Last.ToLower().Contains(name.ToLower()) ||
                                user.Email.ToLower().Contains(name.ToLower())) &&
                                (!role.HasValue || user.UserRoles.Any(r => r.RoleId == role.Value)) &&
                                (user.UserRoles.All(r => r.Role.Alias != DefaultRoles.USER.ToString())))
                 .Include(user => user.UserRoles)
                 .Include("UserRoles.Role")
                 .Include(user => user.Logins)
                 .Include(user => user.Name);

            Query.OrderByDescending(user => user.CreatedOn);


            //switch (sortBy)
            //{
            //    case SortBy.name:
            //        if (sortOrder == SortOrder.desc)
            //            Query.OrderByDescending(user => user.Name.First).ThenByDescending(user => user.Name.Last);
            //        else
            //            Query.OrderBy(user => user.Name.First).ThenBy(user => user.Name.Last);
            //        break;
            //    default:
            //        Query.OrderBy(user => user.Name.First).ThenBy(user => user.Name.Last);
            //        break;
            //}

            Query.Select(user => new UsersListResponseDTO
            {
                id = user.Id,
                first_name = user.Name.First,
                last_name = user.Name.Last,
                email = user.Email,
                role = GetUserRole(user, lang),
                role_alias = user.UserRoles.FirstOrDefault().Role.Alias
            });
        }

        private static string GetUserRole(AppUser user, string lang)
        {
            if (lang.Contains(KeyValueConstants.Arabic))
                return user.UserRoles.FirstOrDefault().Role.NameAr;
            else
                return user.UserRoles.FirstOrDefault().Role.Name;
        }
    }
}
