using ShoppingMall.Api.Admin;
using ShoppingMall.Api.Commodity;
using ShoppingMall.App_Code;
using ShoppingMall.Models.Admin;
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
        private AdminProccess AdminProccessClass;
        private AdminOption AdminOptionClass;
        private CommodityOption CommodityOptionClass;
        private CommodityProccess CommodityProccessClass;

        public CommodityController()
        {
            AdminProccessClass = new AdminProccess();
            AdminOptionClass = new AdminOption();
            CommodityOptionClass = new CommodityOption();
            CommodityProccessClass = new CommodityProccess();
        }

        [Route("getCommodityData")]
        [HttpGet]
        public IHttpActionResult GetCommodityData()
        {
            try
            {
                List<CommodityDataDtoResponse> commodityData = CommodityProccessClass.GetAllCommodityData();

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
        [Route("updateAdminData")]
        [HttpPut]
        public IHttpActionResult UpdateAdminData([FromBody] UpdateAdminDataDto updateData)
        {
            try
            {
                bool inputVaild = AdminProccessClass.CheckUpdateInputData(updateData);

                if (inputVaild)
                {
                    bool result = AdminProccessClass.UpdateAdminData(updateData);

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