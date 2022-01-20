using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ViewModels
{
    public class LoginVM
    {
        public string return_url { get; set; }
        public List<AuthenticationScheme> ExternalProviders { get; set; }
    }
}
