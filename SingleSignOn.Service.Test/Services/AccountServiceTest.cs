using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SingleSignOn.DataAccessLayer.Interfaces;
using SingleSignOn.Domain.Models;
using SingleSignOn.Service.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SingleSignOn.Service.Test.Services
{
    [TestClass]
    public class AccountServiceTest
    {
        private Mock<IIdentityServerInteractionService> _interaction;
        private Mock<IAuthenticationSchemeProvider> _schemeProvider;
        private Mock<IUserRepository> _userRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _interaction = new Mock<IIdentityServerInteractionService>();
            _schemeProvider = new Mock<IAuthenticationSchemeProvider>();
            _userRepository = new Mock<IUserRepository>();
        }

        [TestMethod]
        public void AccountValidateSuccesssTest()
        {
            var applicationUser = new ApplicationUser
            {
                Username = "admin",
                Password = "password",
                SubjectId = "subjectId"
            };
            var accountService = new AccountService(_interaction.Object, _schemeProvider.Object, _userRepository.Object);
            _userRepository.Setup(x => x.GetApplicationUserByCredential(applicationUser.Username, applicationUser.Password)).Returns(Task.FromResult(applicationUser));
            var result = accountService.Validate(applicationUser.Username, applicationUser.Password);
            Assert.AreSame(result.Result, applicationUser);
        }

        [TestMethod]
        public void AccountValidateFailedTest()
        {
            var applicationUser = new ApplicationUser
            {
                Username = "admin",
                Password = "password",
                SubjectId = "subjectId"
            };
            var accountService = new AccountService(_interaction.Object, _schemeProvider.Object, _userRepository.Object);
            _userRepository.Setup(x => x.GetApplicationUserByCredential(applicationUser.Username, applicationUser.Password)).Returns(Task.FromResult(applicationUser));
            var result = accountService.Validate("admin", "wrongpassword");
            Assert.AreNotSame(result.Result, applicationUser);
        }

    }
}
