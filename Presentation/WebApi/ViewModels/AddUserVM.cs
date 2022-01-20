using Application.Interfaces;
using Helpers.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Extensions;

namespace WebApi.ViewModels
{
    public class AddUserVM
    {
        [Required(ErrorMessage = "{0}_IS_REQUIRED")]
        [MaxLength(85, ErrorMessage = "{0}_MAX_LENGTH_IS_{1}")]
        [Display(Name = ResourceKeys.FirstName)]
        public string first_name { get; set; }

        [Required(ErrorMessage = "{0}_IS_REQUIRED")]
        [MaxLength(85, ErrorMessage = "{0}_MAX_LENGTH_IS_{1}")]
        [Display(Name = ResourceKeys.LastName)]
        public string last_name { get; set; }

        [Required(ErrorMessage = "{0}_IS_REQUIRED")]
        [MaxLength(85, ErrorMessage = "{0}_MAX_LENGTH_IS_{1}")]
        [EmailAddress(ErrorMessage = ResourceKeys.InvalidEmail)]
        [Remote(action: "IsValidEmailDomain", controller: "Users")]
        [Display(Name = ResourceKeys.Email)]
        public string email { get; set; }

        public Guid role { get; set; }
    }
}
