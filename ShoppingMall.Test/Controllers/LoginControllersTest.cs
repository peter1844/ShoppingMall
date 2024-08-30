using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingMall.Controllers;
using ShoppingMall.Interface;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Login;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace ShoppingMallTest.Controllers
{
    [TestClass]
    public class LoginControllersTest
    {
        private Mock<ILogin> _mockLogin;
        private Mock<IToken> _mockToken;
        private Mock<ITools> _mockTools;
        private Mock<ILogHelper> _mockLogHelper;
        private Mock<IContextHelper> _mockContextHelper;

        private LoginController loginController;

        [TestInitialize]
        public void Setup()
        {
            _mockLogin = new Mock<ILogin>();
            _mockToken = new Mock<IToken>();
            _mockTools = new Mock<ITools>();
            _mockLogHelper = new Mock<ILogHelper>();
            _mockContextHelper = new Mock<IContextHelper>();

            loginController = new LoginController(_mockLogin.Object, _mockToken.Object, _mockTools.Object, _mockLogHelper.Object, _mockContextHelper.Object);
        }

        [TestMethod]
        [TestCategory("LoginByAccountPassword")]
        public void TestLoginByAccountPasswordSuccess()
        {
            // Arrange
            int dataCount = 1;
            List<AdminUserDataDtoResponse> mockOutputData = new List<AdminUserDataDtoResponse>();

            mockOutputData.Add(new AdminUserDataDtoResponse
            {
                AdminId = 1,
                Name = "Peter",
                Token = "8c1vds6v58df16v1fd65"
            });

            _mockLogin.Setup(cmd => cmd.CheckInputData(It.IsAny<LoginDataDto>())).Returns(true);
            _mockLogin.Setup(cmd => cmd.CheckLoginByAccountPassword(It.IsAny<LoginDataDto>())).Returns(mockOutputData);
            _mockLogin.Setup(cmd => cmd.SetLoginAdminPermissions(mockOutputData[0].AdminId));

            //// Act
            IHttpActionResult result = loginController.LoginByAccountPassword(It.IsAny<LoginDataDto>());

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
        [TestCategory("LoginByAccountPassword")]
        public void TestLoginByAccountPasswordInvaildInput()
        {
            // Arrange
            int dataCount = 1;
            List<AdminUserDataDtoResponse> mockOutputData = new List<AdminUserDataDtoResponse>();

            mockOutputData.Add(new AdminUserDataDtoResponse
            {
                AdminId = 1,
                Name = "Peter",
                Token = "8c1vds6v58df16v1fd65"
            });

            _mockLogin.Setup(cmd => cmd.CheckInputData(It.IsAny<LoginDataDto>())).Returns(false);
            _mockLogin.Setup(cmd => cmd.CheckLoginByAccountPassword(It.IsAny<LoginDataDto>())).Returns(mockOutputData);
            _mockLogin.Setup(cmd => cmd.SetLoginAdminPermissions(mockOutputData[0].AdminId));

            //// Act
            IHttpActionResult result = loginController.LoginByAccountPassword(It.IsAny<LoginDataDto>());

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
        [TestCategory("LoginByAccountPassword")]
        public void TestLoginByAccountPasswordInvaildLogin()
        {
            // Arrange
            int dataCount = 1;
            List<AdminUserDataDtoResponse> mockOutputData = new List<AdminUserDataDtoResponse>();

            _mockLogin.Setup(cmd => cmd.CheckInputData(It.IsAny<LoginDataDto>())).Returns(true);
            _mockLogin.Setup(cmd => cmd.CheckLoginByAccountPassword(It.IsAny<LoginDataDto>())).Returns(mockOutputData);

            //// Act
            IHttpActionResult result = loginController.LoginByAccountPassword(It.IsAny<LoginDataDto>());

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
        [TestCategory("LoginByToken")]
        public void TestLoginByTokenSuccess()
        {
            // Arrange
            _mockToken.Setup(cmd => cmd.CheckLoginByToken()).Returns(true);

            //// Act
            IHttpActionResult result = loginController.LoginByToken();

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
        [TestCategory("LoginByToken")]
        public void TestLoginByTokenInvaildToken()
        {
            // Arrange
            _mockToken.Setup(cmd => cmd.CheckLoginByToken()).Returns(false);

            //// Act
            IHttpActionResult result = loginController.LoginByToken();

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
