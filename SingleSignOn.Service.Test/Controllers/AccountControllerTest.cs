
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SingleSignOn.Domain.Models;
using SingleSignOn.Service.Controllers;
using SingleSignOn.Service.Interfaces;
using SingleSignOn.Service.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SingleSignOn.Service.Test
{
    [TestClass]
    public class AccountControllerTest
    {
        private Mock<IIdentityServerInteractionService> _identityService;
        private Mock<IHttpContextAccessor> _httpContext;
        private Mock<IEventService> _eventService;
        private Mock<IAccountService> _accountService;

        [TestInitialize]
        public void TestInitialize()
        {
            _identityService = new Mock<IIdentityServerInteractionService>();
            _httpContext = new Mock<IHttpContextAccessor>();
            _eventService = new Mock<IEventService>();
            _accountService = new Mock<IAccountService>();
        }

        [TestMethod]
        public void AccountLoginViewTest()
        {

            var accountController = new AccountController(_identityService.Object, _httpContext.Object, _eventService.Object, _accountService.Object );
            var result = accountController.Login("http://www.google.com");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountLoginSuccessfulTest()
        {

            var loginModel = new LoginViewModel
            {
                Username = "admin",
                Password = "password",
                RememberMe = false
            };
            var applicationUser = new ApplicationUser
            {
                Username = "admin",
                Password = "password",
                SubjectId = "subjectId"
            };

            var accountController = new AccountController(_identityService.Object, _httpContext.Object, _eventService.Object, _accountService.Object);
            _accountService.Setup(x => x.Validate(loginModel.Username, loginModel.Password)).Returns(Task.FromResult(applicationUser));
            var result = accountController.Login(loginModel);
            _accountService.Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountLoginFailedTest()
        {

            var loginModel = new LoginViewModel
            {
                Username = "admin",
                Password = "password",
                RememberMe = false
            };

            var accountController = new AccountController(_identityService.Object, _httpContext.Object, _eventService.Object, _accountService.Object);
            _accountService.Setup(x => x.Validate(loginModel.Username, loginModel.Password)).Returns(Task.FromResult((ApplicationUser)null));
            var result = accountController.Login(loginModel).Result as RedirectToRouteResult;
            _accountService.Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AccountLogoutSuccessfulTest()
        {
            var logout = new LogoutViewModel
            {
                LogoutId = "1"
            };

            var accountController = new AccountController(_identityService.Object, _httpContext.Object, _eventService.Object, _accountService.Object);
            var result = accountController.Logout(logout);
            Assert.IsNotNull(result);
        }
    }
}
