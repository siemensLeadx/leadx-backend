using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Context;
using System;
using WebApi.Extensions;
using WebApi.Middlewares;
using Utilities;
using Helpers.Constants;
using Utilities.Services;
using Domain.Entities;
using Application.Interfaces;
using Persistence.SeedData;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;

namespace WebApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllersService();
            services.AddSwaggerService(_configuration);
            services.AddVersioningService();
            services.AddLocalizationService();
            services.AddWebApiServices(_configuration);
            services.AddPersistenceServices(_configuration);
            services.AddUtilitiesServices(_configuration);         
            services.AddCorsOriginService(_configuration);
            services.AddAuthenticationService(_configuration, _env);
            services.AddAuthorizationService();

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 3;

            })
            .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(o => {
                o.LoginPath = new PathString("/dashboard/en/Home/Login");
                o.AccessDeniedPath = new PathString("/dashboard/en/Home/AccessDenied");
                o.LogoutPath = new PathString("/dashboard/en/Home/Logout");
                o.ExpireTimeSpan = TimeSpan.FromDays(1);
               
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        public void Configure(IApplicationBuilder app, IIdentityService identityService)
        {
            AddUsers.SeedData(identityService).GetAwaiter().GetResult();

            app.UseForwardedHeaders(); 
            
            app.UseRequestLocalization();

            // app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseCors(KeyValueConstants.AllowedCrosOrigins);

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
                c.DefaultModelsExpandDepth(-1);
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseMiddleware<ValidateUserPermissionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "LocalizedDefault",
                    pattern: "dashboard/{lang:lang}/{controller=DashboardLeads}/{action=List}/{id?}");

                endpoints.MapFallbackToController("RedirectToDefaultLanguage", "Home");
            });

        }
    }
}
