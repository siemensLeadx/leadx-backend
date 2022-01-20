using Application.Interfaces;
using Helpers.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        private readonly AppSettings _appSettings;
        public ApplicationConfiguration(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }
        public AppSettings GetAppSettings()
        {
            return _appSettings;
        }

        public IEnumerable<ApiClient> GetApiClients()
        {
            return _appSettings.ApiClients;
        }

        public JWTSettings GetJwtSettings()
        {
            return _appSettings.JWTSettings;
        }
    }
}
