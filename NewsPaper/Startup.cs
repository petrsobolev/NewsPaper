using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsPaper.Data;
using NewsPaper.Models;
using NewsPaper.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace NewsPaper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication().AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = "XqDKVP56TIxK7N5SwN5jXwHqP";
                twitterOptions.ConsumerSecret = "ejzbvTHzCckwMU8ZEw9zR5ylU3h1PmwBbgnUZA8VEd7J1wHOmt";
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "240266420061121";
                facebookOptions.AppSecret = "c0ad0a230f1fc84c0e857430f227356c";
            });
            services.AddAuthentication().AddVKontakte(VKOptions =>
            {
                VKOptions.ClientId = "6602917";
                VKOptions.ClientSecret = "Agtld1M81uV3Deb4o8qu";

            });
            services.AddMvc()
                            .AddViewLocalization();            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            var supportedCultures = new[]
            {
                
                new CultureInfo("ru"),
                new CultureInfo("blr")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseStaticFiles();

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
