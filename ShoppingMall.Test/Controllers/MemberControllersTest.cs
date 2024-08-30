using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingMall.Controllers;
using ShoppingMall.Interface;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Enum;
using ShoppingMall.Models.Member;
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
        [TestCategory("GetMemberData")]
        public void TestGetMemberDataSuccess()
        {
            // Arrange
            int dataCount = 2;
            List<MemberDataDtoResponse> mockOutputData = new List<MemberDataDtoResponse>();

            mockOutputData.Add(new MemberDataDtoResponse
            {
                Id = 1,
                Acc = "Guest001",
                Name = "張三",
                Level = 3,
                Enabled = 1,
            });
            mockOutputData.Add(new MemberDataDtoResponse
            {
                Id = 2,
                Acc = "Guest002",
                Name = "李四",
                Level = 5,
                Enabled = 1,
            });

            _mockMember.Setup(cmd => cmd.GetAllMemberData()).Returns(mockOutputData);

            // Act
            IHttpActionResult result = memberController.GetMemberData();

            OkNegotiatedContentResult<List<MemberDataDtoResponse>> correctResponse = result as OkNegotiatedContentResult<List<MemberDataDtoResponse>>;
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
        [TestCategory("UpdateMemberData")]
        public void TestUpdateMemberDataSuccess()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.MemberUpdate)).Returns(true);
            _mockMember.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<UpdateMemberDataDto>())).Returns(true);
            _mockMember.Setup(cmd => cmd.UpdateMemberData(It.IsAny<UpdateMemberDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = memberController.UpdateMemberData(It.IsAny<UpdateMemberDataDto>());

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
        [TestCategory("UpdateMemberData")]
        public void TestUpdateMemberDataNoPermissions()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.MemberUpdate)).Returns(false);
            _mockMember.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<UpdateMemberDataDto>())).Returns(true);
            _mockMember.Setup(cmd => cmd.UpdateMemberData(It.IsAny<UpdateMemberDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = memberController.UpdateMemberData(It.IsAny<UpdateMemberDataDto>());

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
        [TestCategory("UpdateMemberData")]
        public void TestUpdateMemberDataInvaildInput()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.MemberUpdate)).Returns(true);
            _mockMember.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<UpdateMemberDataDto>())).Returns(false);
            _mockMember.Setup(cmd => cmd.UpdateMemberData(It.IsAny<UpdateMemberDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = memberController.UpdateMemberData(It.IsAny<UpdateMemberDataDto>());

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
