using ShoppingMall.Api.Member;
using ShoppingMall.App_Code;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Member;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/member")]
    public class MemberController : ApiController
    {
        private MemberProccess memberProccessClass;

        public MemberController()
        {
            memberProccessClass = new MemberProccess();
        }

        /// <summary>
        /// 取得會員資料
        /// </summary>
        [Route("getMemberData")]
        [HttpGet]
        public IHttpActionResult GetMemberData()
        {
            try
            {
                List<MemberDataDtoResponse> memberData = memberProccessClass.GetAllMemberData();

                return Ok(memberData);
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// 編輯會員資料
        /// </summary>
        [Route("updateMemberData")]
        [HttpPut]
        public IHttpActionResult UpdateMemberData([FromBody] UpdateMemberDataDto updateData)
        {
            try
            {
                bool inputVaild = memberProccessClass.CheckUpdateInputData(updateData);

                if (inputVaild)
                {
                    bool result = memberProccessClass.UpdateMemberData(updateData);

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