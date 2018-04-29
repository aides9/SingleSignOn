using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SingleSignOn.Domain.Models;
using SingleSignOn.Service.Controllers;
using SingleSignOn.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SingleSignOn.Service.Test
{
    [TestClass]
    public class UserControllerTest
    {
        private Mock<IAccountService> _accountService;

        [TestInitialize]
        public void TestInitialize()
        {
            _accountService = new Mock<IAccountService>();
        }

        [TestMethod]
        public void UserCreateSuccessfulTest()
        {
            var applicationUser = new ApplicationUser
            {
                Username = "admin",
                Password = "password",
                SubjectId = "subjectId"
            };

            var userController = new UserController(_accountService.Object);
            _accountService.Setup(x => x.Create(applicationUser)).Returns(Task.FromResult(true));
            var result = userController.Create(applicationUser);
            _accountService.Verify(x => x.Create(It.IsAny<ApplicationUser>()), Times.Once);
            Assert.IsNotNull(result.Value);
        }
    }
}
