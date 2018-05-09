// Unused usings removed.
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AnimeCollections.Models;
using System;
using Microsoft.EntityFrameworkCore;
using AnimeCollections.Data;
using Microsoft.Extensions.Configuration;

namespace AnimeCollections
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                #region Serect Test
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var config = host.Services.GetRequiredService<IConfiguration>();
                var UserPWD = config["UserPWD"];
                var AdminPWD = config["AdminPWD"];

                #endregion

                try
                {

                    SeedData.Initialize(services, UserPWD, AdminPWD).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}