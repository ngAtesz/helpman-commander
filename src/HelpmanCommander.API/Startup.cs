using System;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpmanCommander.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Linq;

namespace HelpmanCommander.API
{
    public class Startup
    {
        private const string OpenApiName = "HelpManCommanderOpenAPISpecification";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                                                        options.UseSqlServer(
                                                            Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICompetitionRepository, CompetitionRepository>();

            services.AddDefaultIdentity<IdentityUser>()
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAutoMapper();

            services.AddSwaggerGen(setupACtion =>
            {
                setupACtion.SwaggerDoc(Configuration.GetValue<string>("OpenApi:Name", "DefaulApiName"), new OpenApiInfo()
                {
                    Title = Configuration.GetValue<string>("OpenApi:DisplayName", "API name missing from configuration"),
                    Version = Configuration.GetValue<string>("OpenApi:Version"),
                    Description = Configuration.GetValue<string>("OpenApi:Description", "Description is missing from configuration"),
                    Contact = new OpenApiContact()
                    {
                        Email = Configuration.GetValue<string>("Contact:Email"),
                        Name = Configuration.GetValue<string>("Contact:Name"),
                        Url = new Uri(Configuration.GetValue<string>("Contact:Url"))
                    }
                });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                setupACtion.IncludeXmlComments(xmlCommentsFullPath, true);
            });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddMvc(setupAction =>
            {
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));

                setupAction.ReturnHttpNotAcceptable = true;

                var jsonOutputFormatter = setupAction.OutputFormatters
                                                    .OfType<JsonOutputFormatter>()
                                                    .FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    // remove text/json as it isn't the approved media type
                    // for working with JSON at API level
                    if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
                    {
                        jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
                    }
                }
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();

            var swaggerUrl = Configuration.GetValue<string>("OpenApi:Url", "n/a");
            var apiName = Configuration.GetValue<string>("OpenApi:DisplayName", "API name missing from configuration");

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    swaggerUrl,
                    apiName);
                setupAction.RoutePrefix = "";
            });

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
