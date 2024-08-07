using ShoppingMall.Api.Commodity;
using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Common;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/commodity")]
    public class CommodityController : ApiController
    {
        private CommodityPermissions commodityPermissionsClass;
        private CommodityProccess commodityProccessClass;
        private CommodityOption commodityOptionClass;
        private CommodityStockCheck commodityStockCheckClass;

        public CommodityController()
        {
            commodityPermissionsClass = new CommodityPermissions();
            commodityProccessClass = new CommodityProccess();
            commodityOptionClass = new CommodityOption();
            commodityStockCheckClass = new CommodityStockCheck();
        }

        /// <summary>
        /// 取得商品頁面權限
        /// </summary>
        [Route("getCommodityPermissions")]
        [HttpGet]
        public IHttpActionResult getCommodityPermissions()
        {
            try
            {
                List<CommodityPermissionsDtoResponse> commodityPermissions = commodityPermissionsClass.GetAllCommodityPermissions();

                return Ok(commodityPermissions);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
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

                List<CommodityDataDtoResponse> commodityData = commodityProccessClass.GetCommodityData(conditionData[0]);

                return Ok(commodityData);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
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
                List<CommodityOptionDataDtoResponse> adminOptionData = commodityOptionClass.GetAllCommodityOptionData();

                return Ok(adminOptionData);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
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
                List<CommodityStockDataDtoResponse> commodityStockData = commodityStockCheckClass.GetShortageCommodityData();

                return Ok(commodityStockData);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
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
                HttpRequest request = HttpContext.Current.Request;
                HttpContext context = HttpContext.Current;

                // 檢查權限
                if (!Tools.CheckPermission((int)Permissions.CommodityInsert)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = commodityProccessClass.CheckInsertInputData(request);

                if (inputVaild)
                {
                    string filePath = "";
                    List<InsertCommodityDataDto> insertData = new List<InsertCommodityDataDto>();

                    if (request.Files.Count > 0)
                    {
                        HttpPostedFile files = request.Files[0];

                        filePath = commodityProccessClass.UploadCommodityFile(files);
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

                    bool result = commodityProccessClass.InsertCommodityData(insertData[0]);

                    return Ok(true);
                }
                else
                {
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
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
                HttpRequest request = HttpContext.Current.Request;

                // 檢查權限
                if (!Tools.CheckPermission((int)Permissions.CommodityUpdate)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = commodityProccessClass.CheckUpdateInputData(request);

                if (inputVaild)
                {
                    string filePath = "";
                    List<UpdateCommodityDataDto> updateData = new List<UpdateCommodityDataDto>();

                    // 如果有手動按下刪除圖片或是上傳新圖片，把舊的圖片刪除
                    if (!string.IsNullOrEmpty(request.Form["DeleteFlag"]) || (request.Files.Count > 0 && !string.IsNullOrEmpty(request.Form["OldImage"])))
                    {
                        commodityProccessClass.DeleteCommodityFile(request.Form["OldImage"]);
                        filePath = "delete";
                    }

                    // 如果有上傳新圖片，執行檔案上傳
                    if (request.Files.Count > 0)
                    {
                        HttpPostedFile files = request.Files[0];

                        filePath = commodityProccessClass.UploadCommodityFile(files);
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

                    bool result = commodityProccessClass.UpdateCommodityData(updateData[0]);

                    return Ok(result);
                }
                else
                {
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }
    }
}