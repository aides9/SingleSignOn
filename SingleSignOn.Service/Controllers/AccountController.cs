using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SingleSignOn.Service.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SingleSignOn.Domain.Models;
using SingleSignOn.Service.Interfaces;
using IdentityServer4.Extensions;
using IdentityModel;

namespace SingleSignOn.Service.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAccountService _account;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IHttpContextAccessor httpContextAccessor,
            IEventService events,
            IAccountService account)
        {
            _interaction = interaction;
            _events = events;
            _account = account;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var vm = new LoginViewModel { ReturnUrl = returnUrl };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            ApplicationUser user = new ApplicationUser();
            if (ModelState.IsValid)
            {
                try
                {
                    // validate username/password against DB
                    user = await _account.Validate(loginModel.Username, loginModel.Password);
                }
                catch
                {
                    ModelState.AddModelError("", "Connection failure, Please Check Database Setting");
                    return View(loginModel);
                }

                if (user != null && user != new ApplicationUser())
                {
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId.ToString(), user.DisplayName));
                    AuthenticationProperties props = null;
                    if (loginModel.RememberMe)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(Config.GetRememberMeLoginDuration())
                        };
                    };

                    // issue authentication cookie with subject ID and username
                    await HttpContext.SignInAsync(user.SubjectId.ToString(), user.Username, props);

                    // make sure the returnUrl is still valid, and if so redirect back to authorize endpoint or a local page
                    if (_interaction.IsValidReturnUrl(loginModel.ReturnUrl) || Url.IsLocalUrl(loginModel.ReturnUrl))
                    {
                        return Redirect(loginModel.ReturnUrl);
                    }

                }

                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(loginModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId };
            return await Logout(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            // build a model so we know what to show on the logout page
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            var user = HttpContext.User;
            if (user?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetName()));
            }

            return View("LoggedOut", vm);
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = logout?.ClientId,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            return vm;
        }
    }
}
