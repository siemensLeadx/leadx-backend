using Application;
using Application.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using Helpers.Constants;
using Helpers.Models;
using WebApi.Services;
using Helpers.Resources;
using MediatR;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Application.Authorization;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddMediatR(Assembly.GetAssembly(typeof(IApplicationLayer)));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PipelineValidationBehavior<,>));
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationRequirementHandler>();
        }

        public static void AddSwaggerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                c.CustomSchemaIds(a => a.FullName);
                c.AddServer(new OpenApiServer { Url = configuration["System:Api_URL"] });
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WebApi",
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri(configuration["License"])
                    }
                });

                c.AddSecurityDefinition("Api-Key", new OpenApiSecurityScheme
                {
                    Name = "X-API-Key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "X-API-Key",
                    BearerFormat = "string",
                    Description = "Input your client api key",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Api-Key",
                            },
                            Scheme = "X-API-Key",
                            Name = "X-API-Key",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }

        public static void AddVersioningService(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }

        public static void AddLocalizationService(this IServiceCollection services)
        {
            services.AddLocalization();

            CultureInfo[] supportedCultures = new[]
            {
                new CultureInfo(KeyValueConstants.ArabicLanguage),
                new CultureInfo(KeyValueConstants.EnglishLanguage),
                new CultureInfo(KeyValueConstants.Arabic),
                new CultureInfo(KeyValueConstants.English)
            };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(KeyValueConstants.EnglishLanguage);
                options.SupportedUICultures = supportedCultures;
            });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            });
        }

        public static void AddControllersService(this IServiceCollection services)
        {
            services.AddControllersWithViews()
            .AddRazorRuntimeCompilation()
            .AddDataAnnotationsLocalization(o => o.DataAnnotationLocalizerProvider = (type, factory) =>
            {
                return factory.Create(typeof(SharedResource));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = new List<ErrorResult>();

                    foreach (var key in context.ModelState.Keys)
                    {
                        var value = context.ModelState[key];
                        foreach (var error in value.Errors)
                        {
                            errors.Add(new ErrorResult { Type = key, Error = error.ErrorMessage });
                        }
                    }

                    var problemDetails = new Error
                    {
                        Errors = errors
                    };

                    return new UnprocessableEntityObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<IApplicationLayer>();
            });
        }

        public static void AddCorsOriginService(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedCrosOrigins = configuration.GetSection("System:AllowedCrosOrigins").Get<string[]>();

            services.AddCors(config =>
            {
                config.AddPolicy(KeyValueConstants.AllowedCrosOrigins,
                    p => //p.//SetIsOriginAllowedToAllowWildcardSubdomains()
                          p.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
                          //.WithOrigins(allowedCrosOrigins)
                          //.AllowCredentials());
            });
        }

        public static void AddAuthenticationService(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment _env)
        {
            

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = KeyValueConstants.Issuer,
                    ValidAudience = KeyValueConstants.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>(KeyValueConstants.SecretHashKey)))
                };

                config.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query[KeyValueConstants.AccessToken];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments(KeyValueConstants.SignalREndPoint))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            })
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = configuration["Microsoft_Auth:Client_Id"];
                microsoftOptions.ClientSecret = configuration["Microsoft_Auth:Client_Secret"];
                microsoftOptions.CallbackPath = "/dashboard/en/signin-microsoft";          
            }); 
        }

        public static void AddAuthorizationService(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(KeyValueConstants.AdminPolicy, policyBuilder =>
                {
                    policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme,
                        IdentityConstants.ApplicationScheme)
                                 .RequireAuthenticatedUser()
                                 .RequireRole(DefaultRoles.ADMIN.ToString());
                });

                options.AddPolicy(KeyValueConstants.HrPolicy, policyBuilder =>
                {
                    policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme,
                        IdentityConstants.ApplicationScheme)
                                 .RequireAuthenticatedUser()
                                 .RequireRole(DefaultRoles.ADMIN.ToString(), DefaultRoles.VIEW_ONLY.ToString());
                });

                options.AddPolicy(KeyValueConstants.UserPolicy, policyBuilder =>
                {
                    policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                 .RequireAuthenticatedUser()
                                 .RequireRole(DefaultRoles.USER.ToString());
                });
            });
        }
    }
}
