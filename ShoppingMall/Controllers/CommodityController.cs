using ShoppingMall.Api.Admin;
using ShoppingMall.Api.Commodity;
using ShoppingMall.App_Code;
using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/commodity")]
    public class CommodityController : ApiController
    {
        private CommodityProccess CommodityProccessClass;
        private CommodityOption CommodityOptionClass;

        public CommodityController()
        {
            CommodityProccessClass = new CommodityProccess();
            CommodityOptionClass = new CommodityOption();
        }

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

                List<CommodityDataDtoResponse> commodityData = CommodityProccessClass.GetCommodityData(conditionData[0]);

                return Ok(commodityData);
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }
        [Route("getCommodityOptionData")]
        [HttpGet]
        public IHttpActionResult GetCommodityOptionData()
        {
            try
            {
                List<CommodityOptionDataDtoResponse> adminOptionData = CommodityOptionClass.GetAllCommodityOptionData();

                return Ok(adminOptionData);
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }
        [Route("insertCommodityData")]
        [HttpPost]
        public IHttpActionResult InsertCommodityData()
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;

                bool inputVaild = CommodityProccessClass.CheckInsertInputData(request);

                if (inputVaild)
                {
                    string filePath = "";
                    List<InsertCommodityDataDto> insertData = new List<InsertCommodityDataDto>();

                    if (request.Files.Count > 0)
                    {
                        HttpPostedFile files = request.Files[0];
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/images/commodity/")))
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/images/commodity"));
                        }

                        files.SaveAs(HttpContext.Current.Server.MapPath($"~/images/commodity/{timestamp}_" + files.FileName));
                        filePath = $"/images/commodity/{timestamp}_{files.FileName}";
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

                    bool result = CommodityProccessClass.InsertCommodityData(insertData[0]);

                    return Ok(true);
                }
                else
                {
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }
        [Route("updateCommodityData")]
        [HttpPut]
        public IHttpActionResult UpdateCommodityData()
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;

                bool inputVaild = CommodityProccessClass.CheckUpdateInputData(request);

                if (inputVaild)
                {
                    string filePath = "";
                    List<UpdateCommodityDataDto> updateData = new List<UpdateCommodityDataDto>();

                    if (request.Files.Count > 0)
                    {
                        HttpPostedFile files = request.Files[0];
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/images/commodity/")))
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/images/commodity"));
                        }

                        files.SaveAs(HttpContext.Current.Server.MapPath($"~/images/commodity/{timestamp}_" + files.FileName));
                        filePath = $"/images/commodity/{timestamp}_{files.FileName}";
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

                    bool result = CommodityProccessClass.UpdateCommodityData(updateData[0]);

                    return Ok(result);
                }
                else
                {
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }
    }
}