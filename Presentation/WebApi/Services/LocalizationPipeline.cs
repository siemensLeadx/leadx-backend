using Helpers.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder app)
        {

            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo(KeyValueConstants.Arabic),
                new CultureInfo(KeyValueConstants.English)
            };

            var options = new RequestLocalizationOptions()
            {

                DefaultRequestCulture = new RequestCulture(KeyValueConstants.EnglishLanguage),
                SupportedUICultures = supportedCultures,
            };

            options.RequestCultureProviders = new[] { 
                new RouteDataRequestCultureProvider() 
                {
                    Options = options,
                    RouteDataStringKey = "lang",
                    UIRouteDataStringKey = "lang" } };

            app.UseRequestLocalization(options);
        }
    }
}
