using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingMall.Controllers;
using ShoppingMall.Interface;
using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Enum;
using ShoppingMall.Models.Order;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace ShoppingMallTest.Controllers
{
    [TestClass]
    public class OrderControllersTest
    {
        private Mock<IOrder> _mockOrder;
        private Mock<ITools> _mockTools;
        private Mock<ILogHelper> _mockLogHelper;

        private OrderController orderController;

        [TestInitialize]
        public void Setup()
        {
            _mockOrder = new Mock<IOrder>();
            _mockTools = new Mock<ITools>();
            _mockLogHelper = new Mock<ILogHelper>();

            orderController = new OrderController(_mockOrder.Object, _mockTools.Object, _mockLogHelper.Object);
        }

        [TestMethod]
        [TestCategory("GetOrderData")]
        public void TestGetOrderDataSuccess()
        {
            // Arrange
            int dataCount = 1;
            List<OrderDataDtoResponse> mockOutputData = new List<OrderDataDtoResponse>();

            mockOutputData.Add(new OrderDataDtoResponse
            {
                Id = "T20240730181357Lv0ecJ",
                MemberName = "李四",
                OrderDate = DateTime.Parse("2024-07-30"),
                PayTypeId = 1,
                PayStateId = 0,
                DeliverTypeId = 1,
                DeliverStateId = 2,
                DeliverStateName = Enum.GetName(typeof(DeliveryStateCode), 2),
                TotalMoney = 600,
                DetailDatas = new List<OrderDetailData> {
                    new OrderDetailData() {
                        CommodityName = "趴在肉上的咖波",
                        Quantity = 6,
                        Price = 100,
                        Image = "20240816093044_1.jpg"
                    }
                }
            });

            _mockOrder.Setup(cmd => cmd.CheckConditionInputData(It.IsAny<OrderConditionDataDto>())).Returns(true);
            _mockOrder.Setup(cmd => cmd.GetOrderData(It.IsAny<OrderConditionDataDto>())).Returns(mockOutputData);

            // Act
            IHttpActionResult result = orderController.GetOrderData(It.IsAny<OrderConditionDataDto>());

            OkNegotiatedContentResult<List<OrderDataDtoResponse>> correctResponse = result as OkNegotiatedContentResult<List<OrderDataDtoResponse>>;
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
        [TestCategory("OrderOptionData")]
        public void TestOrderOptionDataSuccess()
        {
            // Arrange
            int dataCount = 1;
            List<OrderOptionDataDtoResponse> mockOutputData = new List<OrderOptionDataDtoResponse>();

            mockOutputData.Add(new OrderOptionDataDtoResponse
            {
                PayTypes = new List<PayType>{
                    new PayType(){
                        TypeId = 1,
                        TypeName = "Credit"
                    }
                },
                PayStates = new List<PayState>{
                    new PayState(){
                        StateId = 0,
                        StateName = "UnPaid"
                    }
                },
                DeliveryTypes = new List<DeliveryType>{
                    new DeliveryType(){
                        TypeId = 1,
                        TypeName = "LandTransportation"
                    }
                },
                DeliveryStates = new List<DeliveryState>{
                    new DeliveryState(){
                        StateId = 0,
                        StateName = "NotShipped"
                    }
                },
                OpenCommodityDatas = new List<OpenCommodityData>{
                    new OpenCommodityData(){
                        CommodityId = 1,
                        CommodityName = "趴在肉上的咖波",
                        CommodityPrice = 100
                    }
                }

            });

            _mockOrder.Setup(cmd => cmd.GetOrderOptionData()).Returns(mockOutputData);

            // Act
            IHttpActionResult result = orderController.GetOrderOptionData();

            OkNegotiatedContentResult<List<OrderOptionDataDtoResponse>> correctResponse = result as OkNegotiatedContentResult<List<OrderOptionDataDtoResponse>>;
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
        [TestCategory("InsertOrderData")]
        public void TestInsertOrderDataSuccess()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.OrderInsert)).Returns(true);
            _mockOrder.Setup(cmd => cmd.CheckInsertInputData(It.IsAny<InsertOrderDataDto>())).Returns(true);
            _mockOrder.Setup(cmd => cmd.InsertOrderData(It.IsAny<InsertOrderDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = orderController.InsertOrderData(It.IsAny<InsertOrderDataDto>());

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
        [TestCategory("InsertOrderData")]
        public void TestInsertOrderDataNoPermissions()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.OrderInsert)).Returns(false);
            _mockOrder.Setup(cmd => cmd.CheckInsertInputData(It.IsAny<InsertOrderDataDto>())).Returns(true);
            _mockOrder.Setup(cmd => cmd.InsertOrderData(It.IsAny<InsertOrderDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = orderController.InsertOrderData(It.IsAny<InsertOrderDataDto>());

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
        [TestCategory("InsertOrderData")]
        public void TestInsertOrderDataInvaildInput()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.OrderInsert)).Returns(true);
            _mockOrder.Setup(cmd => cmd.CheckInsertInputData(It.IsAny<InsertOrderDataDto>())).Returns(false);
            _mockOrder.Setup(cmd => cmd.InsertOrderData(It.IsAny<InsertOrderDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = orderController.InsertOrderData(It.IsAny<InsertOrderDataDto>());

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
        [TestCategory("UpdateOrderData")]
        public void TestUpdateOrderDataSuccess()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.OrderUpdate)).Returns(true);
            _mockOrder.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<UpdateOrderDataDto>())).Returns(true);
            _mockOrder.Setup(cmd => cmd.UpdateOrderData(It.IsAny<UpdateOrderDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = orderController.UpdateOrderData(It.IsAny<UpdateOrderDataDto>());

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
        [TestCategory("UpdateOrderData")]
        public void TestUpdateOrderDataNoPermissions()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.OrderUpdate)).Returns(false);
            _mockOrder.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<UpdateOrderDataDto>())).Returns(true);
            _mockOrder.Setup(cmd => cmd.UpdateOrderData(It.IsAny<UpdateOrderDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = orderController.UpdateOrderData(It.IsAny<UpdateOrderDataDto>());

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
        [TestCategory("UpdateOrderData")]
        public void TestUpdateOrderDataInvaildInput()
        {
            // Arrange
            _mockTools.Setup(cmd => cmd.CheckPermission((int)Permissions.OrderUpdate)).Returns(true);
            _mockOrder.Setup(cmd => cmd.CheckUpdateInputData(It.IsAny<UpdateOrderDataDto>())).Returns(false);
            _mockOrder.Setup(cmd => cmd.UpdateOrderData(It.IsAny<UpdateOrderDataDto>())).Returns(true);

            // Act
            IHttpActionResult result = orderController.UpdateOrderData(It.IsAny<UpdateOrderDataDto>());

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
