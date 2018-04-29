using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SingleSignOn.Domain.Models;
using SingleSignOn.Service.Interfaces;

namespace SingleSignOn.Service.Controllers
{
    public class UserController : Controller
    {
        private readonly IAccountService _account;
        public UserController(
           IAccountService account)
        {
            _account = account;
        }

        [HttpPost]
        [Route("User/Create")]
        public JsonResult Create([FromBody] ApplicationUser applicationUser)
        {
            var _success = _account.Create(applicationUser);
            return Json(new { success = _success });
        }

    }
}