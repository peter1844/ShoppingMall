using ShoppingMall.Api.Member;
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
        private MemberProccess MemberProccessClass;

        public MemberController()
        {
            MemberProccessClass = new MemberProccess();
        }

        [Route("getMemberData")]
        [HttpGet]
        public IHttpActionResult GetMemberData()
        {
            try
            {
                List<MemberDataDtoResponse> memberData = MemberProccessClass.GetAllMemberData();

                return Ok(memberData);
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { StatusErrorCode = ex.Message });
            }
        }
        [Route("updateMemberData")]
        [HttpPut]
        public IHttpActionResult UpdateMemberData([FromBody] UpdateMemberDataDto updateData)
        {
            try
            {
                bool inputVaild = MemberProccessClass.CheckUpdateInputData(updateData);

                if (inputVaild)
                {
                    bool result = MemberProccessClass.UpdateMemberData(updateData);

                    return Ok(result);
                }
                else
                {
                    return Ok(new ExceptionData { StatusErrorCode = "A101" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ExceptionData { StatusErrorCode = ex.Message });
            }
        }
    }
}