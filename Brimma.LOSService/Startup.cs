using Brimma.LOSService.Common;
using Brimma.LOSService.Config;
using Brimma.LOSService.Extensions;
using Brimma.LOSService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Brimma.LOSService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            if (IsNUnitRunning())
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var builder = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
                Configuration = builder.Build();
                StaticConfig = builder.Build();
            }
            else
            {
                Configuration = configuration;
                StaticConfig = configuration;
            }
        }
        public static IConfiguration StaticConfig { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddLocalization(o =>
            {
                // We will put our translations in a folder called Resources
                o.ResourcesPath = "Resources";
            });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<EncompassAPIs>(Configuration.GetSection("EncompassAPIs"));
            services.Configure<FloifyServiceAPIs>(Configuration.GetSection("FloifyServiceAPIs"));
            services.Configure<AuthServiceAPIs>(Configuration.GetSection("AuthServiceAPIs"));
            services.Configure<Cosmos>(Configuration.GetSection("Cosmos"));
            services.Configure<AzureQueue>(Configuration.GetSection("AzureQueue"));
            services.Configure<ErrorMessages>(Configuration.GetSection("ErrorMessages"));
            services.Configure<OrderOutLoanFieldIds>(Configuration.GetSection("OrderOutLoanFieldIds"));
            services.Configure<OrderOutTitleEscrowEmailFieldIds>(Configuration.GetSection("OrderOutTitleEscrowEmailFieldIds"));
            services.Configure<OrderOutAppraisalOrderFieldIds>(Configuration.GetSection("OrderOutAppraisalOrderFieldIds"));
            services.Configure<LoanConfiguration>(Configuration.GetSection("LoanConfiguration"));
            services.Configure<ProspectConfiguration>(Configuration.GetSection("ProspectConfiguration"));
            services.Configure<EPPSConfiguration>(Configuration.GetSection("EPPSConfiguration"));

            services.AddSingleton<IHttpService, HttpService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IBorrowerService, BorrowerService>();
            services.AddTransient<SaveNLog, SaveNLog>();
            services.AddTransient<AsymmetricRSAAlgorithm, AsymmetricRSAAlgorithm>();
            services.AddTransient<IApplicationInsights, ApplicationInsights>();


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "OAuth2 Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Encompass API",
                    Version = "v1",
                    Description = "Encompass API document",
                    Contact = new OpenApiContact
                    {
                        Name = "Brimma Tech"
                    },
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Encompass API",
                    Version = "v2",
                    Description = "Encompass API document",
                    Contact = new OpenApiContact
                    {
                        Name = "Brimma Tech"
                    },
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);


            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddHttpContextAccessor();
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Encompass API V1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Encompass API V2");
                options.RoutePrefix = string.Empty;
            });

            app.UseCustomExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
        private static bool IsNUnitRunning()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Any(assembly => assembly.FullName.ToLowerInvariant().StartsWith("nunit.framework"));
        }
    }
}
