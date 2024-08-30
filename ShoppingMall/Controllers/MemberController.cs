using Newtonsoft.Json;
using ShoppingMall.Interface;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Enum;
using ShoppingMall.Models.Member;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/member")]
    public class MemberController : ApiController
    {
        private IMember _member;
        private ITools _tools;
        private ILogHelper _logHelper;

        public MemberController(IMember member, ITools tools, ILogHelper logHelper)
        {
            _member = member;
            _tools = tools;
            _logHelper = logHelper;
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
                List<MemberDataDtoResponse> memberData = _member.GetAllMemberData();

                return Ok(memberData);
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
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
                _logHelper.Info(JsonConvert.SerializeObject(updateData));

                // 檢查權限
                if (!_tools.CheckPermission((int)Permissions.MemberUpdate)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = _member.CheckUpdateInputData(updateData);

                if (inputVaild)
                {
                    bool result = _member.UpdateMemberData(updateData);

                    return Ok(result);
                }
                else
                {
                    _logHelper.Warn(JsonConvert.SerializeObject(updateData));
                    return Ok(new ExceptionData { ErrorMessage = StateCode.InvaildInputData.ToString() });
                }
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.InnerException.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }
    }
}