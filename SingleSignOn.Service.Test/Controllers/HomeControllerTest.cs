using IdentityServer4.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SingleSignOn.Service.Controllers;

namespace SingleSignOn.Service.Test
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void HomeIndexTest()
        {
            Mock<IIdentityServerInteractionService> _service = new Mock<IIdentityServerInteractionService>();
            var homeController = new HomeController(_service.Object);
            var result = homeController.Index();
            Assert.IsNotNull(result);
        }
    }
}
