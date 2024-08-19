using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingMall.Controllers;
using ShoppingMall.Interface;
using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Enum;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace ShoppingMallTest.Controllers
{
    [TestClass]
    public class AdminControllersTest
    {
        private Mock<IAdmin> _mockAdmin;
        private Mock<ITools> _mockTools;
        private Mock<ILogHelper> _mockLogHelper;

        private AdminController adminController;

        [TestInitialize]
        public void Setup()
        {
            _mockAdmin = new Mock<IAdmin>();
            _mockTools = new Mock<ITools>();
            _mockLogHelper = new Mock<ILogHelper>();
            adminController = new AdminController(_mockAdmin.Object, _mockTools.Object, _mockLogHelper.Object);
        }

        [TestMethod]
        public void TestGetAdminData()
        {
            // Arrange
            int dataCount = 2;
            List<AdminUserDataDtoResponse> mockData = new List<AdminUserDataDtoResponse>();

            mockData.Add(new AdminUserDataDtoResponse
            {
                Id = 1,
                Acc = "r1Kaq34Nuu",
                Name = "Peter",
                Enabled = 1,
                Role = new List<AdminUserRoleData> { new AdminUserRoleData() { RoleId = 2 } }
            });
            mockData.Add(new AdminUserDataDtoResponse
            {
                Id = 8,
                Acc = "pA15eFvv69N",
                Name = "Burger",
                Enabled = 1,
                Role = new List<AdminUserRoleData> { 
                    new AdminUserRoleData() { RoleId = 2 },
                    new AdminUserRoleData() { RoleId = 4 }
                }
            });

            _mockAdmin.Setup(cmd => cmd.GetAllAdminUserData()).Returns(mockData);

            // Act
            OkNegotiatedContentResult<List<AdminUserDataDtoResponse>> result = adminController.GetAdminData() as OkNegotiatedContentResult<List<AdminUserDataDtoResponse>>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataCount, result.Content.Count);
        }

        [TestMethod]
        public void TestInsertAdminData()
        {
            // Arrange
            //int dataCount = 2;
            List<InsertAdminDataDto> mockData = new List<InsertAdminDataDto>();

            mockData.Add(new InsertAdminDataDto
            {
                Name = "TestAdmin1",
                Acc = "uZ42gYk70s",
                Pwd = "123456",
                Roles = new List<int> { 1, 2, 3 },
                Enabled = 1,
            });

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminInsert)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckInsertInputData(mockData[0])).Returns(false);
            _mockAdmin.Setup(cmd => cmd.InsertAdminData(mockData[0])).Returns(true);

            // Act
            OkNegotiatedContentResult<bool> result = adminController.InsertAdminData(mockData[0]) as OkNegotiatedContentResult<bool>;

            // Assert
            //Assert.IsNotNull(result);
            Assert.IsTrue(result.Content);
            //Assert.AreEqual(dataCount, result.Content.Count);
        }
    }
}
