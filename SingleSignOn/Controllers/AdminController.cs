using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SingleSignOn.Enum;
using SingleSignOn.Models;

namespace SingleSignOn.Controllers
{
    public class AdminController : Controller
    {
       
        [Authorize(Policy = "Admin")] // Add admin policy filter based to allow only admin role claim only
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create([Bind(include: "Username, Password, ConfirmPassword, DisplayName, Role")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                setUserViewModel(ref userViewModel);
                var success = await handleSendRequest(userViewModel);
                if (success)
                {
                    return View("index", new UserViewModel());
                }
            }
            
            return View("index", userViewModel);
        }

        private async Task<bool> handleSendRequest(UserViewModel userViewModel)
        {
            //send create account request to server
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = new HttpResponseMessage();
                client.BaseAddress = new Uri("http://localhost:6002");
                var content = new StringContent(JsonConvert.SerializeObject(userViewModel).ToString(), Encoding.UTF8, "application/json");
                try
                {
                    response = await client.PostAsync("/user/create", content);
                }
                catch (Exception)
                {
                    ViewData["Message"] = "Service Error";
                }
                if (response.IsSuccessStatusCode)
                {
                    ViewData["Message"] = "Create Success";
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    ViewData["Message"] = "Service is Offline";
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var results = JsonConvert.DeserializeObject<ErrorViewModel>(data);
                    ViewData["Message"] = results.Message;
                }

                return false;
            }
        }

        private void setUserViewModel(ref UserViewModel userViewModel)
        {
            var currentUser = User.Claims.Where(c => c.Type == "name").Select(c => c.Value).SingleOrDefault();
            userViewModel.CreatedBy = currentUser;
            userViewModel.CreatedOn = DateTime.Now;
            userViewModel.ModifiedBy = currentUser;
            userViewModel.ModifiedOn = DateTime.Now;
            userViewModel.SubjectId = Guid.NewGuid().ToString();
        }
    }
}