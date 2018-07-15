using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NewsPaper.Models;

namespace NewsPaper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            var host = BuildWebHost(args);
            Task.Run(async () =>
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        await RoleInitializer.InitializeAsync(userManager, rolesManager);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }
            }).GetAwaiter().GetResult();

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
