using ShoppingMall.Api.Member;
using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Member;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/member")]
    public class MemberController : ApiController
    {
        private MemberPermissions memberPermissionsClass;
        private MemberProccess memberProccessClass;

        public MemberController()
        {
            memberPermissionsClass = new MemberPermissions();
            memberProccessClass = new MemberProccess();
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
                LogHelper.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
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
                LogHelper.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
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
                LogHelper.Info(JsonConvert.SerializeObject(updateData));

                // 檢查權限
                if (!Tools.CheckPermission((int)Permissions.MemberUpdate)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

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
                LogHelper.Warn(ex.InnerException.Message);
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
            }
        }
    }
}