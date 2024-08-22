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
    public class LogoutControllersTest
    {
        private Mock<ILogout> _mockLogout;
        private Mock<ITools> _mockTools;
        private Mock<ILogHelper> _mockLogHelper;

        private LogoutController logoutController;

        [TestInitialize]
        public void Setup()
        {
            _mockLogout = new Mock<ILogout>();
            _mockTools = new Mock<ITools>();
            _mockLogHelper = new Mock<ILogHelper>();

            logoutController = new LogoutController(_mockLogout.Object, _mockTools.Object, _mockLogHelper.Object);
        }

        [TestMethod]
        public void TestLogoutProccess()
        {
            // Arrange
            _mockLogout.Setup(cmd => cmd.LogoutProccess());

            //// Act
            IHttpActionResult result = logoutController.LogoutProccess();

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
