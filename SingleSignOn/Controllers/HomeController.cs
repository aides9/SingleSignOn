using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SingleSignOn.Enum;
using SingleSignOn.Models;

namespace SingleSignOn.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize]
        public IActionResult Index()
        {
            return RedirectByRole();
        }
        
        public IActionResult RedirectByRole()
        {
            //Get role claims and redirect user base on role
            var role = User.Claims.Where(c => c.Type.Equals("role")).Select(c => c.Value).SingleOrDefault();
            Role parsedRole;

            //Redirect user base on role
            if (System.Enum.TryParse(role, out parsedRole))
            {
                return RedirectToRoute(new { controller = parsedRole.ToString(), action = "Index" });
            }

            //Guest and non-defined role will redirect to home default view
            return View();
        }

        public IActionResult Claims()
        {
            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
