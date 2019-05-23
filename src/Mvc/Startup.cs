using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Mvc
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
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Home/Index/");
                    options.AccessDeniedPath = new PathString("/Account/Forbidden/");
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.ExpireTimeSpan = new TimeSpan(2, 0, 0);
                })
                .AddOpenIdConnect(o =>
                {
                    //o.Authority = Configuration["oidc:authority"];
                    //o.ClientId = Configuration["oidc:clientid"];
                    //o.ClientSecret = Configuration["oidc:clientsecret"];

                    //o.Authority = "http://localhost:5000";
                    o.Authority = "http://azurewebsites.net";
                    o.ClientId = "mvc";
                    o.ClientSecret = "senha";

                    o.RequireHttpsMetadata = false;

                    o.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                    o.SaveTokens = true;
                    o.GetClaimsFromUserInfoEndpoint = true;
                    o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.Scope.Add("openid");
                    o.Scope.Add("profile");
                    o.Scope.Add("company");
                    o.Scope.Add("api");

                    o.ClaimActions.MapAllExcept("aud", "iss", "iat", "nbf", "exp", "aio", "c_hash", "uti", "nonce");

                    o.Events = new OpenIdConnectEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.HandleResponse();

                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            //if (Environment.IsDevelopment())
                            //{
                            //// Debug only, in production do not share exceptions with the remote host.
                            //return c.Response.WriteAsync(c.Exception.ToString());
                            //}
                            return c.Response.WriteAsync(c.Exception.ToString());
                            //return c.Response.WriteAsync("An error occurred processing your authentication.");
                        }
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //app.UseCookiePolicy();

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
