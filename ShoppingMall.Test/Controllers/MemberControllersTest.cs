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
        public void TestGetMemberData()
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
        public void TestUpdateMemberData()
        {
            // Arrange
            List<UpdateMemberDataDto> mockInputData = new List<UpdateMemberDataDto>();

            mockInputData.Add(new UpdateMemberDataDto
            {
                MemberId = 1,
                Level = 2,
                Enabled = 0,
            });

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.MemberUpdate)).Returns(true);
            _mockMember.Setup(cmd => cmd.CheckUpdateInputData(mockInputData[0])).Returns(true);
            _mockMember.Setup(cmd => cmd.UpdateMemberData(mockInputData[0])).Returns(true);

            // Act
            IHttpActionResult result = memberController.UpdateMemberData(mockInputData[0]);

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
