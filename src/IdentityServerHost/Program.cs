using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Quickstart.UI;
using IdentityServerHost.Data;
using IdentityServerHost.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace IdentityServerHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "IdentityServer4";

            var host = BuildWebHost(args);

            //Console.WriteLine("Seeding database...");

            using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //Update-Database -Context PersistedGrantDbContext
                //scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

                //Update-Database -Context ConfigurationDbContext
                //scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();

                //Update-Database -Context ApplicationDbContext

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                EnsureSeedData(context);

                //var application = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //EnsureSeedUser(application);

                //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                //var bob = new ApplicationUser
                //{
                //    UserName = "bob"
                //};
                //var result = userManager.CreateAsync(bob, "Pass123$").Result;

                //result = userManager.AddClaimsAsync(bob, new Claim[]
                //{
                //    new Claim(JwtClaimTypes.Subject, "Bob Smith"),
                //    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                //    new Claim(JwtClaimTypes.GivenName, "Bob"),
                //    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                //    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                //    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                //    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                //    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                //    new Claim(JwtClaimTypes.IdentityProvider, "idsvr"),
                //    new Claim(JwtClaimTypes.AuthenticationTime, DateTimeOffset.UtcNow.ToEpochTime().ToString(), ClaimValueTypes.Integer),
                //    new Claim("business_id", "5")
                //}).Result;
            }

            //Console.WriteLine("Done seeding database.");

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIISIntegration()
                .CaptureStartupErrors(true)
                .UseStartup<Startup>()
                //.UseSerilog((context, configuration) =>
                //{
                //    configuration
                //        .MinimumLevel.Debug()
                //        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                //        .MinimumLevel.Override("System", LogEventLevel.Warning)
                //        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                //        .Enrich.FromLogContext()
                //        .WriteTo.File(@"identityserver4_log.txt")
                //        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                //})
                .Build();
        }

        //public static List<IdentityUser> Users = new List<IdentityUser>
        //{
        //    new IdentityUser{ SubjectId = "818727", UserName = "alice", Password = "alice",
        //        Claims =
        //        {
        //            new Claim(JwtClaimTypes.Name, "Alice Smith"),
        //            new Claim(JwtClaimTypes.GivenName, "Alice"),
        //            new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //            new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
        //            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
        //            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
        //            new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
        //            new Claim("business_id", "4")
        //        }
        //    },
        //    new TestUser{SubjectId = "88421113", Username = "bob", Password = "bob",
        //        Claims =
        //        {
        //            new Claim(JwtClaimTypes.Name, "Bob Smith"),
        //            new Claim(JwtClaimTypes.GivenName, "Bob"),
        //            new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //            new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
        //            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
        //            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
        //            new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
        //            new Claim("business_id", "5")
        //        }
        //    }
        //};

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Console.WriteLine("Clients being populated");
                foreach (var client in Config.GetClients().ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!context.ApiResources.Any())
            {
                Console.WriteLine("ApiResources being populated");
                foreach (var resource in Config.GetApis().ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }
        }
    }
}