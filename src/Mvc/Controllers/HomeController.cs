using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Mvc.Models;

namespace Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Your application description page.";

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/Home/Index"
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult Logout()
        {
            //for signout
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "Home/Index"
            }, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> CallApi()
        {
            //ViewData["Message"] = "Your contact page.";

            //return View();

            //var apiUri = "http://localhost:5002/api/values";

            //var at = await HttpContext.Authentication.GetTokenAsync

            //var client = new HttpClient();

            //var response = await client.GetAsync(apiUri);
            //if (response.IsSuccessStatusCode)
            //{
            //    var json = await response.Content.ReadAsStringAsync();
            //    ViewData["json"] = json;
            //}
            //else
            //{
            //    ViewData["json"] = response.StatusCode;
            //}

            using (var client = new System.Net.Http.HttpClient())
            {
                Microsoft.AspNetCore.Http.Authentication.AuthenticationManager authentication = HttpContext.Authentication;
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await HttpContext.GetTokenAsync("access_token"));

                var id_token = HttpContext.GetTokenAsync("access_token").Result;

                //var response = await client.GetAsync("http://localhost:5002/api/values");
                var response = await client.GetAsync("http://hiveidentityserver.azurewebsites.net/api/values");
                if (!response.IsSuccessStatusCode)
                {
                    return View("About");
                }
            }

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
