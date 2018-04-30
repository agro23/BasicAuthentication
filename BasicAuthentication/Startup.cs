using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BasicAuthentication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BasicAuthentication
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json"); 
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddEntityFrameworkMySql()
                    .AddDbContext<BasicAuthenticationContext>(options =>
                                              options
                                                   .UseMySql(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<BasicAuthenticationContext>()
                .AddDefaultTokenProviders();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = false; // Always want an uppercase except for testing
                options.Password.RequireNonAlphanumeric = false; // make them work for it usually
                options.Password.RequiredLength = 0;  // reset this to longer for security
                options.Password.RequireDigit = false;
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Index}/{id?}");  
            });

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Something went wrong!");
            });
        }
    }
}
