using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingMall.Controllers;
using ShoppingMall.Interface;
using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Enum;
using System.Collections.Generic;
using System.Web.Http;
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
            List<AdminUserDataDtoResponse> mockOutputData = new List<AdminUserDataDtoResponse>();

            mockOutputData.Add(new AdminUserDataDtoResponse
            {
                Id = 1,
                Acc = "r1Kaq34Nuu",
                Name = "Peter",
                Enabled = 1,
                Role = new List<AdminUserRoleData> { new AdminUserRoleData() { RoleId = 2 } }
            });
            mockOutputData.Add(new AdminUserDataDtoResponse
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

            _mockAdmin.Setup(cmd => cmd.GetAllAdminUserData()).Returns(mockOutputData);

            // Act
            IHttpActionResult result = adminController.GetAdminData();

            OkNegotiatedContentResult<List<AdminUserDataDtoResponse>> correctResponse = result as OkNegotiatedContentResult<List<AdminUserDataDtoResponse>>;
            OkNegotiatedContentResult<ExceptionData> ExceptionResponse = result as OkNegotiatedContentResult<ExceptionData>;

            // Assert
            if (correctResponse == null)
            {
                // 拋出Exception
                Assert.Fail(ExceptionResponse.Content.ErrorMessage);
            }
            else
            {
                // 正常回傳
                Assert.AreEqual(dataCount, correctResponse.Content.Count);
            }
        }

        [TestMethod]
        public void TestGetAdminOptionData()
        {
            // Arrange
            int dataCount = 2;
            List<AdminOptionDataDtoResponse> mockOutputData = new List<AdminOptionDataDtoResponse>();

            mockOutputData.Add(new AdminOptionDataDtoResponse
            {
                RoleId = 2,
                RoleName = "Admin",
            });
            mockOutputData.Add(new AdminOptionDataDtoResponse
            {
                RoleId = 3,
                RoleName = "CustomerService",
            });

            _mockAdmin.Setup(cmd => cmd.GetAllAdminOptionData()).Returns(mockOutputData);

            // Act
            IHttpActionResult result = adminController.GetAdminOptionData();

            OkNegotiatedContentResult<List<AdminOptionDataDtoResponse>> correctResponse = result as OkNegotiatedContentResult<List<AdminOptionDataDtoResponse>>;
            OkNegotiatedContentResult<ExceptionData> ExceptionResponse = result as OkNegotiatedContentResult<ExceptionData>;

            // Assert
            if (correctResponse == null)
            {
                // 拋出Exception
                Assert.Fail(ExceptionResponse.Content.ErrorMessage);
            }
            else
            {
                // 正常回傳
                Assert.AreEqual(dataCount, correctResponse.Content.Count);
            }
        }

        [TestMethod]
        public void TestInsertAdminData()
        {
            // Arrange
            List<InsertAdminDataDto> mockInputData = new List<InsertAdminDataDto>();

            mockInputData.Add(new InsertAdminDataDto
            {
                Name = "TestAdmin1",
                Acc = "uZ42gYk70s",
                Pwd = "123456",
                Roles = new List<int> { 1, 2, 3 },
                Enabled = 1,
            });

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminInsert)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckInsertInputData(mockInputData[0])).Returns(true);
            _mockAdmin.Setup(cmd => cmd.InsertAdminData(mockInputData[0])).Returns(true);

            // Act
            IHttpActionResult result = adminController.InsertAdminData(mockInputData[0]);

            OkNegotiatedContentResult<bool> correctResponse = result as OkNegotiatedContentResult<bool>;
            OkNegotiatedContentResult<ExceptionData> ExceptionResponse = result as OkNegotiatedContentResult<ExceptionData>;

            // Assert
            if (correctResponse == null)
            {
                // 拋出Exception
                Assert.Fail(ExceptionResponse.Content.ErrorMessage);
            }
            else
            {
                // 正常回傳
                Assert.IsTrue(correctResponse.Content);
            }
        }

        [TestMethod]
        public void TestUpdateAdminData()
        {
            // Arrange
            List<UpdateAdminDataDto> mockInputData = new List<UpdateAdminDataDto>();

            mockInputData.Add(new UpdateAdminDataDto
            {
                AdminId = 2,
                Name = "TestAdmin1",
                Pwd = "123456",
                Roles = new List<int> { 1, 2, 3 },
                Enabled = 0,
            });

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminUpdate)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckUpdateInputData(mockInputData[0])).Returns(true);
            _mockAdmin.Setup(cmd => cmd.UpdateAdminData(mockInputData[0])).Returns(true);

            // Act
            IHttpActionResult result = adminController.UpdateAdminData(mockInputData[0]);

            OkNegotiatedContentResult<bool> correctResponse = result as OkNegotiatedContentResult<bool>;
            OkNegotiatedContentResult<ExceptionData> ExceptionResponse = result as OkNegotiatedContentResult<ExceptionData>;

            // Assert
            if (correctResponse == null)
            {
                // 拋出Exception
                Assert.Fail(ExceptionResponse.Content.ErrorMessage);
            }
            else
            {
                // 正常回傳
                Assert.IsTrue(correctResponse.Content);
            }
        }

        [TestMethod]
        public void TestDeleteAdminData()
        {
            // Arrange
            List<DeleteAdminDataDto> mockInputData = new List<DeleteAdminDataDto>();

            mockInputData.Add(new DeleteAdminDataDto
            {
                AdminId = 2
            });

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminDelete)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckDeleteInputData(mockInputData[0])).Returns(true);
            _mockAdmin.Setup(cmd => cmd.DeleteAdminData(mockInputData[0])).Returns(true);

            // Act
            IHttpActionResult result = adminController.DeleteAdminData(mockInputData[0]);

            OkNegotiatedContentResult<bool> correctResponse = result as OkNegotiatedContentResult<bool>;
            OkNegotiatedContentResult<ExceptionData> ExceptionResponse = result as OkNegotiatedContentResult<ExceptionData>;

            // Assert
            if (correctResponse == null)
            {
                // 拋出Exception
                Assert.Fail(ExceptionResponse.Content.ErrorMessage);
            }
            else
            {
                // 正常回傳
                Assert.IsTrue(correctResponse.Content);
            }
        }
    }
}
