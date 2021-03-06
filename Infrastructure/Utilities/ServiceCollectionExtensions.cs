using Application.Interfaces;
using Helpers.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Mail;
using Utilities.Services;

namespace Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static void  AddUtilitiesServices(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("System:EmailSettings").Get<EmailSettings>();

            services.Configure<EmailSettings>(configuration.GetSection("System:EmailSettings"));
            services.Configure<ApiClient>(configuration.GetSection("System:ApiClients"));
            services.Configure<AppSettings>(configuration.GetSection("System"));
            services.Configure<FirebaseSetting>(configuration.GetSection("Firebase"));

            services
                .AddFluentEmail(emailSettings.FromEmail)
                .AddRazorRenderer()
                .AddSmtpSender(emailSettings.Host, emailSettings.Port, emailSettings.FromEmail, emailSettings.Password);

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IApplicationConfiguration, ApplicationConfiguration>();
            services.AddScoped<IPhoneValidator, PhoneValidator>();
            services.AddScoped<IApplicationLocalization, ApplicationLocalization>();
            services.AddScoped<IFirebaseMessageSender, FirebaseMessageSender>();
            services.AddSingleton<IExcelOperations, ExcelOperations>();
        }
    }
}
