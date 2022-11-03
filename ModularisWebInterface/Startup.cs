using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModularisWebInterface.Models;
using ModularisWebInterface.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ModularisWebInterface.Models.UserManagement;
using ModularisWebInterface.Models.Accessors;
using Microsoft.AspNetCore.Http;
using ModularisWebInterface.Models.UserManagement.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ModularisWebInterface.Models.UserManagement.Confirmation;
using ModularisWebInterface.Models.UserManagement.Helper;

namespace ModularisWebInterface
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connString = Configuration.GetConnectionString("Release");
            if (WebHostEnvironment.IsDevelopment())
                Configuration.GetConnectionString("DefaultConnection");

            //services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddScoped<IUserConfirmation<IdentityUser>, ManualConfirmation>();

            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config =>
                {

                });
            /*services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                if (WebHostEnvironment.IsDevelopment())
                {
                    config.Password.RequiredLength = 4;
                    config.Password.RequireDigit = false;
                    config.Password.RequireUppercase = false;
                }
                else
                {
                    config.Password.RequiredLength = 8;
                    config.Password.RequireDigit = true;
                    config.Password.RequireUppercase = true;
                }
                config.Password.RequireNonAlphanumeric = false;
                config.SignIn.RequireConfirmedAccount = true;
            })
                //.AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();*/

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
            });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("UserManagement", policyBuilder =>
                {
                    policyBuilder.RequireClaim("UserManager");
                });
                config.AddPolicy("BotManagement", policyBuilder =>
                {
                    policyBuilder.RequireClaim("BotManager");
                    policyBuilder.RequireAuthenticatedUser();
                });
            });

            services.AddSignalR();
            services.AddControllersWithViews();

            services.AddSingleton<IModuleManager, ModuleManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            //services.AddScoped<UserHelper>();

            services.AddSingleton<MainHub, MainHub>();

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            Console.WriteLine(Environment.CurrentDirectory);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MainHub>("/mainhub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
