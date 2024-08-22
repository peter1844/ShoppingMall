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
    public class MemberControllersTest
    {
        private Mock<IMember> _mockMember;
        private Mock<ITools> _mockTools;
        private Mock<ILogHelper> _mockLogHelper;

        private MemberController memberController;

        [TestInitialize]
        public void Setup()
        {
            _mockMember = new Mock<IMember>();
            _mockTools = new Mock<ITools>();
            _mockLogHelper = new Mock<ILogHelper>();
            memberController = new MemberController(_mockMember.Object, _mockTools.Object, _mockLogHelper.Object);
        }

        [TestMethod]
        public void TestGetMemberData()
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
            IHttpActionResult result = adminController.GetAdminData();

            OkNegotiatedContentResult<List<AdminUserDataDtoResponse>> correctResponse = result as OkNegotiatedContentResult<List<AdminUserDataDtoResponse>>;
            OkNegotiatedContentResult<ExceptionData> ExceptionResponse = result as OkNegotiatedContentResult<ExceptionData>;

            // Assert
            if (correctResponse == null)
            {
                // 拋出Exception
                Assert.Fail(ExceptionResponse.Content.ErrorMessage);
            }
            else {
                // 正常回傳
                Assert.AreEqual(dataCount, correctResponse.Content.Count);
            }
        }

        [TestMethod]
        public void TestGetAdminOptionData()
        {
            // Arrange
            int dataCount = 2;
            List<AdminOptionDataDtoResponse> mockData = new List<AdminOptionDataDtoResponse>();

            mockData.Add(new AdminOptionDataDtoResponse
            {
                RoleId = 2,
                RoleName = "Admin",
            });
            mockData.Add(new AdminOptionDataDtoResponse
            {
                RoleId = 3,
                RoleName = "CustomerService",
            });

            _mockAdmin.Setup(cmd => cmd.GetAllAdminOptionData()).Returns(mockData);

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
            _mockAdmin.Setup(cmd => cmd.CheckInsertInputData(mockData[0])).Returns(true);
            _mockAdmin.Setup(cmd => cmd.InsertAdminData(mockData[0])).Returns(true);

            // Act
            IHttpActionResult result = adminController.InsertAdminData(mockData[0]);

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
            List<UpdateAdminDataDto> mockData = new List<UpdateAdminDataDto>();

            mockData.Add(new UpdateAdminDataDto
            {
                AdminId = 2,
                Name = "TestAdmin1",
                Pwd = "123456",
                Roles = new List<int> { 1, 2, 3 },
                Enabled = 0,
            });

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminUpdate)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckUpdateInputData(mockData[0])).Returns(true);
            _mockAdmin.Setup(cmd => cmd.UpdateAdminData(mockData[0])).Returns(true);

            // Act
            IHttpActionResult result = adminController.UpdateAdminData(mockData[0]);

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
            List<DeleteAdminDataDto> mockData = new List<DeleteAdminDataDto>();

            mockData.Add(new DeleteAdminDataDto
            {
                AdminId = 2
            });

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.AdminDelete)).Returns(true);
            _mockAdmin.Setup(cmd => cmd.CheckDeleteInputData(mockData[0])).Returns(true);
            _mockAdmin.Setup(cmd => cmd.DeleteAdminData(mockData[0])).Returns(true);

            // Act
            IHttpActionResult result = adminController.DeleteAdminData(mockData[0]);

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
