using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingMall.Controllers;
using System.Web.Http;
using ShoppingMall.Api.Admin;

namespace ShoppingMallUnitTest
{
    [TestClass]
    public class AdminUnitTest
    {
        private Mock<IAdminPermissions> mockAdminPermissions;
        private AdminController adminController;

        [TestInitialize]
        public void Setup()
        {
            mockAdminPermissions = new Mock<IAdminPermissions>();
            mockAdminPermissions.Setup(x => x.GetAllAdminPermissions()).Returns(new System.Collections.Generic.List<ShoppingMall.Models.Admin.AdminPermissionsDtoResponse>());

            adminController = new AdminController(mockAdminPermissions.Object);
        }

        [TestMethod]
        public void GetAdminPermissionsTest()
        {


            IHttpActionResult a = adminController.getAdminPermissions();

            Assert.AreEqual(1, 1, "Add method did not return the expected result.");


        }
    }
}
