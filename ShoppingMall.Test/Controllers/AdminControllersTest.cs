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
        [TestCategory("GetAdminData")]
        public void TestGetAdminDataSuccess()
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
        [TestCategory("GetAdminOptionData")]
        public void TestGetAdminOptionDataSuccess()
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
        [TestCategory("InsertAdminData")]
        public void TestInsertAdminDataSuccess()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminInsert)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckInsertInputData(It.IsAny<InsertAdminDataDto>())).Returns(true);
            _mockAdmin.Setup(cmd => cmd.InsertAdminData(It.IsAny<InsertAdminDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = adminController.InsertAdminData(It.IsAny<InsertAdminDataDto>());

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
        [TestCategory("InsertAdminData")]
        public void TestInsertAdminDataNoPermissions()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminInsert)).Returns(false);
            _mockAdmin.Setup(cmd => cmd.CheckInsertInputData(It.IsAny<InsertAdminDataDto>())).Returns(true);
            _mockAdmin.Setup(cmd => cmd.InsertAdminData(It.IsAny<InsertAdminDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = adminController.InsertAdminData(It.IsAny<InsertAdminDataDto>());

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
        [TestCategory("InsertAdminData")]
        public void TestInsertAdminDataInvaildInput()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminInsert)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckInsertInputData(It.IsAny<InsertAdminDataDto>())).Returns(false);
            _mockAdmin.Setup(cmd => cmd.InsertAdminData(It.IsAny<InsertAdminDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = adminController.InsertAdminData(It.IsAny<InsertAdminDataDto>());

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
        [TestCategory("UpdateAdminData")]
        public void TestUpdateAdminDataSuccess()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminUpdate)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<UpdateAdminDataDto>())).Returns(true);
            _mockAdmin.Setup(cmd => cmd.UpdateAdminData(It.IsAny<UpdateAdminDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = adminController.UpdateAdminData(It.IsAny<UpdateAdminDataDto>());

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
        [TestCategory("UpdateAdminData")]
        public void TestUpdateAdminDataNoPermissions()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminUpdate)).Returns(false);
            _mockAdmin.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<UpdateAdminDataDto>())).Returns(true);
            _mockAdmin.Setup(cmd => cmd.UpdateAdminData(It.IsAny<UpdateAdminDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = adminController.UpdateAdminData(It.IsAny<UpdateAdminDataDto>());

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
        [TestCategory("UpdateAdminData")]
        public void TestUpdateAdminDataInvaildInput()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminUpdate)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<UpdateAdminDataDto>())).Returns(false);
            _mockAdmin.Setup(cmd => cmd.UpdateAdminData(It.IsAny<UpdateAdminDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = adminController.UpdateAdminData(It.IsAny<UpdateAdminDataDto>());

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
        [TestCategory("DeleteAdminData")]
        public void TestDeleteAdminDataSuccess()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminDelete)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckDeleteInputData(It.IsAny<DeleteAdminDataDto>())).Returns(true);
            _mockAdmin.Setup(cmd => cmd.DeleteAdminData(It.IsAny<DeleteAdminDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = adminController.DeleteAdminData(It.IsAny<DeleteAdminDataDto>());

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
        [TestCategory("DeleteAdminData")]
        public void TestDeleteAdminDataNoPermissions()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminDelete)).Returns(false);
            _mockAdmin.Setup(cmd => cmd.CheckDeleteInputData(It.IsAny<DeleteAdminDataDto>())).Returns(true);
            _mockAdmin.Setup(cmd => cmd.DeleteAdminData(It.IsAny<DeleteAdminDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = adminController.DeleteAdminData(It.IsAny<DeleteAdminDataDto>());

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
        [TestCategory("DeleteAdminData")]
        public void TestDeleteAdminDataInvaildInput()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminDelete)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckDeleteInputData(It.IsAny<DeleteAdminDataDto>())).Returns(false);
            _mockAdmin.Setup(cmd => cmd.DeleteAdminData(It.IsAny<DeleteAdminDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = adminController.DeleteAdminData(It.IsAny<DeleteAdminDataDto>());

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
