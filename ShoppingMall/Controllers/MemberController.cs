using ShoppingMall.Api.Member;
using ShoppingMall.App_Code;
using ShoppingMall.Base;
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
        private MemberPermissions memberPermissionsClass;
        private MemberProccess memberProccessClass;
        private Base.Base baseClass;

        public MemberController()
        {
            memberPermissionsClass = new MemberPermissions();
            memberProccessClass = new MemberProccess();
            baseClass = new Base.Base();
        }

        /// <summary>
        /// 取得會員頁面權限
        /// </summary>
        [Route("getMemberPermissions")]
        [HttpGet]
        public IHttpActionResult getMemberPermissions()
        {
            try
            {
                List<MemberPermissionsDtoResponse> memberPermissions = memberPermissionsClass.GetAllMemberPermissions();

                return Ok(memberPermissions);
            }
            catch (Exception ex)
            {
                Base.Base.Logger(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
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
                Base.Base.Logger(ex.Message);
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
                // 檢查權限
                if (!baseClass.CheckPermission((int)Permissions.MemberUpdate)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

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
                Base.Base.Logger(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = ex.Message });
            }
        }
    }
}