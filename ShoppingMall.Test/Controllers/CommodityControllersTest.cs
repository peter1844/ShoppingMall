using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingMall.Controllers;
using ShoppingMall.Interface;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Enum;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace ShoppingMallTest.Controllers
{
    [TestClass]
    public class CommodityControllersTest
    {
        private Mock<ICommodity> _mockCommodity;
        private Mock<ITools> _mockTools;
        private Mock<ILogHelper> _mockLogHelper;
        private Mock<IContextHelper> _mockContextHelper;

        private CommodityController commodityController;

        [TestInitialize]
        public void Setup()
        {
            _mockCommodity = new Mock<ICommodity>();
            _mockTools = new Mock<ITools>();
            _mockLogHelper = new Mock<ILogHelper>();
            _mockContextHelper = new Mock<IContextHelper>();

            commodityController = new CommodityController(_mockCommodity.Object, _mockTools.Object, _mockLogHelper.Object, _mockContextHelper.Object);
        }

        [TestMethod]
        [TestCategory("GetCommodityData")]
        public void TestGetCommodityDataSuccess()
        {
            // Arrange
            int dataCount = 1;
            List<CommodityDataDtoResponse> mockOutputData = new List<CommodityDataDtoResponse>();

            mockOutputData.Add(new CommodityDataDtoResponse
            {
                Id = 1,
                Name = "趴在肉上的咖波",
                Description = "無肉不歡，沒肉就歡的咖波來啦!!!!!",
                Type = 3,
                Image = "20240816093044_1.jpg",
                Price = 100,
                Stock = 11,
                Open = 1,
                CommodityName = "小孩玩具",
            });

            _mockCommodity.Setup(cmd => cmd.GetCommodityData(It.IsAny<ConditionDataDto>())).Returns(mockOutputData);

            // Act
            IHttpActionResult result = commodityController.GetCommodityData(It.IsAny<ConditionDataDto>());

            OkNegotiatedContentResult<List<CommodityDataDtoResponse>> correctResponse = result as OkNegotiatedContentResult<List<CommodityDataDtoResponse>>;
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
        [TestCategory("GetCommodityOptionData")]
        public void TestGetCommodityOptionDataSuccess()
        {
            // Arrange
            int dataCount = 2;
            List<CommodityOptionDataDtoResponse> mockOutputData = new List<CommodityOptionDataDtoResponse>();

            mockOutputData.Add(new CommodityOptionDataDtoResponse
            {
                CommodityId = 1,
                CommodityName = "日常用品" 
            });
            mockOutputData.Add(new CommodityOptionDataDtoResponse
            {
                CommodityId = 2,
                CommodityName = "醫療"
            });

            _mockCommodity.Setup(cmd => cmd.GetAllCommodityOptionData()).Returns(mockOutputData);

            // Act
            IHttpActionResult result = commodityController.GetCommodityOptionData();

            OkNegotiatedContentResult<List<CommodityOptionDataDtoResponse>> correctResponse = result as OkNegotiatedContentResult<List<CommodityOptionDataDtoResponse>>;
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
        [TestCategory("GetCommodityOptionData")]
        public void TestCheckCommodityStockSuccess()
        {
            // Arrange
            int dataCount = 1;
            List<CommodityStockDataDtoResponse> mockOutputData = new List<CommodityStockDataDtoResponse>();

            mockOutputData.Add(new CommodityStockDataDtoResponse
            {
                InventoryShortageCount = 4
            });

            _mockCommodity.Setup(cmd => cmd.GetShortageCommodityData()).Returns(mockOutputData);

            // Act
            IHttpActionResult result = commodityController.CheckCommodityStock();

            OkNegotiatedContentResult<List<CommodityStockDataDtoResponse>> correctResponse = result as OkNegotiatedContentResult<List<CommodityStockDataDtoResponse>>;
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
        [TestCategory("InsertCommodityData")]
        public void TestInsertCommodityDataSuccess()
        {
            // Arrange
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            Mock<HttpFileCollectionBase> mockFileCollection = new Mock<HttpFileCollectionBase>();

            // 模擬Form內容
            NameValueCollection mockFormCollection = new NameValueCollection
            {
                { "Name", "角落生物-粉粉的" },
                { "Description", "" },
                { "Type", "4" },
                { "Price", "1000" },
                { "Stock", "9" },
                { "Open", "1" }
            };
            mockRequest.Setup(r => r.Form).Returns(mockFormCollection);

            // 模擬上傳檔案
            mockFile.Setup(f => f.FileName).Returns("test.txt");
            mockFileCollection.Setup(c => c.Count).Returns(1);
            mockFileCollection.Setup(c => c[0]).Returns(mockFile.Object);
            mockRequest.Setup(r => r.Files).Returns(mockFileCollection.Object);

            _mockContextHelper.Setup(cmd => cmd.GetHttpRequest()).Returns(mockRequest.Object);

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.CommodityInsert)).Returns(true);
            _mockCommodity.Setup(cmd => cmd.CheckInsertInputData(It.IsAny<HttpRequestBase>())).Returns(true);
            _mockCommodity.Setup(cmd => cmd.UploadCommodityFile(It.IsAny<HttpPostedFileBase>())).Returns("20240826141341_1.png");
            _mockCommodity.Setup(cmd => cmd.InsertCommodityData(It.IsAny<InsertCommodityDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = commodityController.InsertCommodityData();

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
        [TestCategory("InsertCommodityData")]
        public void TestInsertCommodityDataNoPermissions()
        {
            // Arrange
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            Mock<HttpFileCollectionBase> mockFileCollection = new Mock<HttpFileCollectionBase>();

            // 模擬Form內容
            NameValueCollection mockFormCollection = new NameValueCollection
            {
                { "Name", "角落生物-粉粉的" },
                { "Description", "" },
                { "Type", "4" },
                { "Price", "1000" },
                { "Stock", "9" },
                { "Open", "1" }
            };
            mockRequest.Setup(r => r.Form).Returns(mockFormCollection);

            // 模擬上傳檔案
            mockFile.Setup(f => f.FileName).Returns("test.txt");
            mockFileCollection.Setup(c => c.Count).Returns(1);
            mockFileCollection.Setup(c => c[0]).Returns(mockFile.Object);
            mockRequest.Setup(r => r.Files).Returns(mockFileCollection.Object);

            _mockContextHelper.Setup(cmd => cmd.GetHttpRequest()).Returns(mockRequest.Object);

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.CommodityInsert)).Returns(false);
            _mockCommodity.Setup(cmd => cmd.CheckInsertInputData(It.IsAny<HttpRequestBase>())).Returns(true);
            _mockCommodity.Setup(cmd => cmd.UploadCommodityFile(It.IsAny<HttpPostedFileBase>())).Returns("20240826141341_1.png");
            _mockCommodity.Setup(cmd => cmd.InsertCommodityData(It.IsAny<InsertCommodityDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = commodityController.InsertCommodityData();

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
        [TestCategory("InsertCommodityData")]
        public void TestInsertCommodityDataInvaildInput()
        {
            // Arrange
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            Mock<HttpFileCollectionBase> mockFileCollection = new Mock<HttpFileCollectionBase>();

            // 模擬Form內容
            NameValueCollection mockFormCollection = new NameValueCollection
            {
                { "Name", "角落生物-粉粉的" },
                { "Description", "" },
                { "Type", "4" },
                { "Price", "1000" },
                { "Stock", "9" },
                { "Open", "1" }
            };
            mockRequest.Setup(r => r.Form).Returns(mockFormCollection);

            // 模擬上傳檔案
            mockFile.Setup(f => f.FileName).Returns("test.txt");
            mockFileCollection.Setup(c => c.Count).Returns(1);
            mockFileCollection.Setup(c => c[0]).Returns(mockFile.Object);
            mockRequest.Setup(r => r.Files).Returns(mockFileCollection.Object);

            _mockContextHelper.Setup(cmd => cmd.GetHttpRequest()).Returns(mockRequest.Object);

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.CommodityInsert)).Returns(true);
            _mockCommodity.Setup(cmd => cmd.CheckInsertInputData(It.IsAny<HttpRequestBase>())).Returns(false);
            _mockCommodity.Setup(cmd => cmd.UploadCommodityFile(It.IsAny<HttpPostedFileBase>())).Returns("20240826141341_1.png");
            _mockCommodity.Setup(cmd => cmd.InsertCommodityData(It.IsAny<InsertCommodityDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = commodityController.InsertCommodityData();

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
        [TestCategory("UpdateCommodityData")]
        public void TestUpdateCommodityDataSuccess()
        {
            // Arrange
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            Mock<HttpFileCollectionBase> mockFileCollection = new Mock<HttpFileCollectionBase>();

            // 模擬Form內容
            NameValueCollection mockFormCollection = new NameValueCollection
            {
                { "CommodityId", "4" },
                { "Name", "咖波娃娃" },
                { "Description", "療癒小物" },
                { "Type", "3" },
                { "Price", "500" },
                { "Stock", "10" },
                { "Open", "1" },
                { "OldImage", "20240723151913_1.png" },
                { "DeleteFlag", "1" }
            };
            mockRequest.Setup(r => r.Form).Returns(mockFormCollection);

            // 模擬上傳檔案
            mockFile.Setup(f => f.FileName).Returns("test.txt");
            mockFileCollection.Setup(c => c.Count).Returns(1);
            mockFileCollection.Setup(c => c[0]).Returns(mockFile.Object);
            mockRequest.Setup(r => r.Files).Returns(mockFileCollection.Object);

            _mockContextHelper.Setup(cmd => cmd.GetHttpRequest()).Returns(mockRequest.Object);

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.CommodityUpdate)).Returns(true);
            _mockCommodity.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<HttpRequestBase>())).Returns(true);
            _mockCommodity.Setup(cmd => cmd.UploadCommodityFile(It.IsAny<HttpPostedFileBase>())).Returns("20240826141341_1.png");
            _mockCommodity.Setup(cmd => cmd.UpdateCommodityData(It.IsAny<UpdateCommodityDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = commodityController.UpdateCommodityData();

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
        [TestCategory("UpdateCommodityData")]
        public void TestUpdateCommodityDataNoPermissions()
        {
            // Arrange
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            Mock<HttpFileCollectionBase> mockFileCollection = new Mock<HttpFileCollectionBase>();

            // 模擬Form內容
            NameValueCollection mockFormCollection = new NameValueCollection
            {
                { "CommodityId", "4" },
                { "Name", "咖波娃娃" },
                { "Description", "療癒小物" },
                { "Type", "3" },
                { "Price", "500" },
                { "Stock", "10" },
                { "Open", "1" },
                { "OldImage", "20240723151913_1.png" },
                { "DeleteFlag", "1" }
            };
            mockRequest.Setup(r => r.Form).Returns(mockFormCollection);

            // 模擬上傳檔案
            mockFile.Setup(f => f.FileName).Returns("test.txt");
            mockFileCollection.Setup(c => c.Count).Returns(1);
            mockFileCollection.Setup(c => c[0]).Returns(mockFile.Object);
            mockRequest.Setup(r => r.Files).Returns(mockFileCollection.Object);

            _mockContextHelper.Setup(cmd => cmd.GetHttpRequest()).Returns(mockRequest.Object);

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.CommodityUpdate)).Returns(false);
            _mockCommodity.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<HttpRequestBase>())).Returns(true);
            _mockCommodity.Setup(cmd => cmd.UploadCommodityFile(It.IsAny<HttpPostedFileBase>())).Returns("20240826141341_1.png");
            _mockCommodity.Setup(cmd => cmd.UpdateCommodityData(It.IsAny<UpdateCommodityDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = commodityController.UpdateCommodityData();

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
        [TestCategory("UpdateCommodityData")]
        public void TestUpdateCommodityDataInvaildInput()
        {
            // Arrange
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            Mock<HttpFileCollectionBase> mockFileCollection = new Mock<HttpFileCollectionBase>();

            // 模擬Form內容
            NameValueCollection mockFormCollection = new NameValueCollection
            {
                { "CommodityId", "4" },
                { "Name", "咖波娃娃" },
                { "Description", "療癒小物" },
                { "Type", "3" },
                { "Price", "500" },
                { "Stock", "10" },
                { "Open", "1" },
                { "OldImage", "20240723151913_1.png" },
                { "DeleteFlag", "1" }
            };
            mockRequest.Setup(r => r.Form).Returns(mockFormCollection);

            // 模擬上傳檔案
            mockFile.Setup(f => f.FileName).Returns("test.txt");
            mockFileCollection.Setup(c => c.Count).Returns(1);
            mockFileCollection.Setup(c => c[0]).Returns(mockFile.Object);
            mockRequest.Setup(r => r.Files).Returns(mockFileCollection.Object);

            _mockContextHelper.Setup(cmd => cmd.GetHttpRequest()).Returns(mockRequest.Object);

            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.CommodityUpdate)).Returns(true);
            _mockCommodity.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<HttpRequestBase>())).Returns(false);
            _mockCommodity.Setup(cmd => cmd.UploadCommodityFile(It.IsAny<HttpPostedFileBase>())).Returns("20240826141341_1.png");
            _mockCommodity.Setup(cmd => cmd.UpdateCommodityData(It.IsAny<UpdateCommodityDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = commodityController.UpdateCommodityData();

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
