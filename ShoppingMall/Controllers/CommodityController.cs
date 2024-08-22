using Newtonsoft.Json;
using ShoppingMall.Api.Admin;
using ShoppingMall.Api.Commodity;
using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Interface;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Enum;
using ShoppingMall.Views;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/commodity")]
    public class CommodityController : ApiController
    {
        private ICommodity _commodity;
        private ITools _tools;
        private ILogHelper _logHelper;
        private IContextHelper _contextHelper;

        public CommodityController()
        {
            _commodity = new CommodityProccess();
            _tools = new Tools();
            _logHelper = new LogHelper();
            _contextHelper = new ContextHelper();
        }

        public CommodityController(ICommodity commodity, ITools tools, ILogHelper logHelper, IContextHelper contextHelper)
        {
            _commodity = commodity;
            _tools = tools;
            _logHelper = logHelper;
            _contextHelper = contextHelper;
        }

        /// <summary>
        /// 取得商品資料
        /// </summary>
        [Route("getCommodityData")]
        [HttpGet]
        public IHttpActionResult GetCommodityData()
        {
            try
            {
                List<ConditionDataDto> conditionData = new List<ConditionDataDto>();
                HttpRequest request = HttpContext.Current.Request;

                conditionData.Add(new ConditionDataDto
                {
                    Name = string.IsNullOrEmpty(request.QueryString["name"]) ? "" : request.QueryString["name"],
                    Type = string.IsNullOrEmpty(request.QueryString["Type"]) ? 0 : Convert.ToInt32(request.QueryString["Type"])
                });

                _logHelper.Info(JsonConvert.SerializeObject(conditionData));

                List<CommodityDataDtoResponse> commodityData = _commodity.GetCommodityData(conditionData[0]);

                return Ok(commodityData);
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }

        /// <summary>
        /// 取得商品頁面的選項
        /// </summary>
        [Route("getCommodityOptionData")]
        [HttpGet]
        public IHttpActionResult GetCommodityOptionData()
        {
            try
            {
                List<CommodityOptionDataDtoResponse> adminOptionData = _commodity.GetAllCommodityOptionData();

                return Ok(adminOptionData);
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }

        /// <summary>
        /// 取得庫存不足商品資料
        /// </summary>
        [Route("checkCommodityStock")]
        [HttpGet]
        public IHttpActionResult CheckCommodityStock()
        {
            try
            {
                List<CommodityStockDataDtoResponse> commodityStockData = _commodity.GetShortageCommodityData();

                return Ok(commodityStockData);
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }

        /// <summary>
        /// 新增商品資料
        /// </summary>
        [Route("insertCommodityData")]
        [HttpPost]
        public IHttpActionResult InsertCommodityData()
        {
            try
            {
                HttpContextBase context = _contextHelper.GetContext();
                HttpRequestBase request = context.Request;

                // 檢查權限
                if (!_tools.CheckPermission((int)Permissions.CommodityInsert)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = _commodity.CheckInsertInputData(request);

                if (inputVaild)
                {
                    string filePath = "";
                    List<InsertCommodityDataDto> insertData = new List<InsertCommodityDataDto>();

                    if (request.Files.Count > 0)
                    {
                        HttpPostedFileBase files = request.Files[0];

                        filePath = _commodity.UploadCommodityFile(files);
                    }

                    insertData.Add(new InsertCommodityDataDto
                    {
                        Name = request.Form["Name"],
                        Description = request.Form["Description"],
                        Type = Convert.ToInt32(request.Form["Type"]),
                        Price = Convert.ToInt32(request.Form["Price"]),
                        Stock = Convert.ToInt32(request.Form["Stock"]),
                        Open = Convert.ToInt32(request.Form["Open"]),
                        ImagePath = filePath
                    });

                    _logHelper.Info(JsonConvert.SerializeObject(insertData));

                    bool result = _commodity.InsertCommodityData(insertData[0]);

                    return Ok(result);
                }
                else
                {
                    _logHelper.Warn(JsonConvert.SerializeObject(request.Form));
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }

        /// <summary>
        /// 編輯商品資料
        /// </summary>
        [Route("updateCommodityData")]
        [HttpPut]
        public IHttpActionResult UpdateCommodityData()
        {
            try
            {
                HttpContextBase context = _contextHelper.GetContext();
                HttpRequestBase request = context.Request;

                // 檢查權限
                if (!_tools.CheckPermission((int)Permissions.CommodityUpdate)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = _commodity.CheckUpdateInputData(request);

                if (inputVaild)
                {
                    string filePath = "";
                    List<UpdateCommodityDataDto> updateData = new List<UpdateCommodityDataDto>();

                    // 如果有手動按下刪除圖片或是上傳新圖片，把舊的圖片刪除
                    if (!string.IsNullOrEmpty(request.Form["DeleteFlag"]) || (request.Files.Count > 0 && !string.IsNullOrEmpty(request.Form["OldImage"])))
                    {
                        _commodity.DeleteCommodityFile(request.Form["OldImage"]);
                        filePath = "delete";
                    }

                    // 如果有上傳新圖片，執行檔案上傳
                    if (request.Files.Count > 0)
                    {
                        HttpPostedFileBase files = request.Files[0];

                        filePath = _commodity.UploadCommodityFile(files);
                    }

                    updateData.Add(new UpdateCommodityDataDto
                    {
                        CommodityId = Convert.ToInt32(request.Form["CommodityId"]),
                        Name = request.Form["Name"],
                        Description = request.Form["Description"],
                        Type = Convert.ToInt32(request.Form["Type"]),
                        Price = Convert.ToInt32(request.Form["Price"]),
                        Stock = Convert.ToInt32(request.Form["Stock"]),
                        Open = Convert.ToInt32(request.Form["Open"]),
                        ImagePath = filePath
                    });

                    _logHelper.Info(JsonConvert.SerializeObject(updateData));

                    bool result = _commodity.UpdateCommodityData(updateData[0]);

                    return Ok(result);
                }
                else
                {
                    _logHelper.Warn(JsonConvert.SerializeObject(request.Form));
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }
    }
}