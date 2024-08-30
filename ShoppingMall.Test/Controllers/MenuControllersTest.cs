using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingMall.Controllers;
using ShoppingMall.Interface;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Menu;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace ShoppingMallTest.Controllers
{
    [TestClass]
    public class MenuControllersTest
    {
        private Mock<IMenu> _mockMenu;
        private Mock<ITools> _mockTools;
        private Mock<ILogHelper> _mockLogHelper;

        private MenuController menuController;

        [TestInitialize]
        public void Setup()
        {
            _mockMenu = new Mock<IMenu>();
            _mockTools = new Mock<ITools>();
            _mockLogHelper = new Mock<ILogHelper>();

            menuController = new MenuController(_mockMenu.Object, _mockTools.Object, _mockLogHelper.Object);
        }

        [TestMethod]
        [TestCategory("SetLanguage")]
        public void TestSetLanguageSuccess()
        {
            // Arrange
            List<MenuLanguageDto> mockInputData = new List<MenuLanguageDto>();

            mockInputData.Add(new MenuLanguageDto
            {
                Language = "tw"
            });

            _mockMenu.Setup(cmd => cmd.SetLanguage(mockInputData[0].Language));

            // Act
            IHttpActionResult result = menuController.SetLanguage(mockInputData[0]);

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
