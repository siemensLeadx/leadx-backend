using Domain.Common;
using Domain.ValueObjects;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppUser : IdentityUser<Guid>, IBaseEntity
    {
        protected AppUser()
        {
            Claims = new HashSet<AppUserClaim>();
            Logins = new HashSet<AppUserLogin>();
            Tokens = new HashSet<AppUserToken>();
            UserRoles = new HashSet<AppUserRole>();
            Devices = new HashSet<Device>();
            Leads = new HashSet<Lead>();
            Notifications = new HashSet<Notification>();
        }

        public AppUser(Name name, string user_name, string email,
            string phone_number = null, string profile_picture = null, bool is_active = true) : base()
        {
            Name = name;
            UserName = user_name;
            NormalizedUserName = user_name.ToUpper();
            Email = email;
            NormalizedEmail = email.ToUpper();
            PhoneNumber = phone_number;
            ProfilePicture = profile_picture;
            IsActive = is_active;

            Claims = new HashSet<AppUserClaim>();
            Logins = new HashSet<AppUserLogin>();
            Tokens = new HashSet<AppUserToken>();
            UserRoles = new HashSet<AppUserRole>();
            Devices = new HashSet<Device>();
            Leads = new HashSet<Lead>();
            Notifications = new HashSet<Notification>();
        }

        public void EditData(Name name, string email, string phoneNumber, string profilePicture)
        {
            Name = name == null ? Name : name;
            UserName = string.IsNullOrWhiteSpace(email) ? UserName : email;
            NormalizedUserName = string.IsNullOrWhiteSpace(email) ? NormalizedUserName : email.ToUpper();
            Email = string.IsNullOrWhiteSpace(email) ? Email : email;
            NormalizedEmail = string.IsNullOrWhiteSpace(email) ? NormalizedEmail : email.ToUpper();
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? PhoneNumber : phoneNumber;
            ProfilePicture = string.IsNullOrWhiteSpace(profilePicture) ? ProfilePicture : profilePicture;
        }

        public Name Name { get; private set; }
        public string ProfilePicture { get; private set; }
        public bool IsActive { get; private set; }
        public virtual ICollection<AppUserClaim> Claims { get; set; }
        public virtual ICollection<AppUserLogin> Logins { get; set; }
        public virtual ICollection<AppUserToken> Tokens { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<Lead> Leads { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
