using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SingleSignOn.Controllers
{
    public class BusinessUserController : Controller
    {
        [Authorize(Policy = "BusinessUser")]
        public IActionResult Index()
        {
            return View();
        }
    }
}