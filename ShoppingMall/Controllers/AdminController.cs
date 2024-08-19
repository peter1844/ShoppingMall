using Newtonsoft.Json;
using ShoppingMall.Api.Admin;
using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Interface;
using ShoppingMall.Models.Admin;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ShoppingMall.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        private IAdmin _admin;
        private ITools _tools;
        private ILogHelper _logHelper;

        public AdminController()
        {
            _admin = new AdminProccess();
            _tools = new Tools();
            _logHelper = new LogHelper();
        }

        public AdminController(IAdmin admin, ITools tools, ILogHelper logHelper)
        {
            _admin = admin;
            _tools = tools;
            _logHelper = logHelper;
        }

        /// <summary>
        /// 取得管理者資料
        /// </summary>
        [Route("getAdminData")]
        [HttpGet]
        public IHttpActionResult GetAdminData()
        {
            try
            {
                List<AdminUserDataDtoResponse> adminUserData = _admin.GetAllAdminUserData();

                return Ok(adminUserData);
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }

        /// <summary>
        /// 取得管理者頁面所需的選項
        /// </summary>
        /// <returns></returns>
        [Route("getAdminOptionData")]
        [HttpGet]
        public IHttpActionResult GetAdminOptionData()
        {
            try
            {
                List<AdminOptionDataDtoResponse> adminOptionData = _admin.GetAllAdminOptionData();

                return Ok(adminOptionData);
            }
            catch (Exception ex)
            {
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }

        /// <summary>
        /// 新增管理者資料
        /// </summary>
        [Route("insertAdminData")]
        [HttpPost]
        public IHttpActionResult InsertAdminData([FromBody] InsertAdminDataDto insertData)
        {
            try
            {
                _logHelper.Info(JsonConvert.SerializeObject(insertData));

                // 檢查權限
                if (!_tools.CheckPermission((int)Permissions.AdminInsert)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = _admin.CheckInsertInputData(insertData);

                if (inputVaild)
                {
                    bool result = _admin.InsertAdminData(insertData);

                    return Ok(result);
                }
                else
                {
                    _logHelper.Warn(JsonConvert.SerializeObject(insertData));
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
        /// 編輯管理者資料
        /// </summary>
        [Route("updateAdminData")]
        [HttpPut]
        public IHttpActionResult UpdateAdminData([FromBody] UpdateAdminDataDto updateData)
        {
            try
            {
                _logHelper.Info(JsonConvert.SerializeObject(updateData));

                // 檢查權限
                if (!_tools.CheckPermission((int)Permissions.AdminUpdate)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = _admin.CheckUpdateInputData(updateData);

                if (inputVaild)
                {
                    bool result = _admin.UpdateAdminData(updateData);

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
                _logHelper.Error(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = _tools.ReturnExceptionMessage(ex.Message) });
            }
        }

        /// <summary>
        /// 刪除管理者資料
        /// </summary>
        [Route("deleteAdminData")]
        [HttpDelete]
        public IHttpActionResult DeleteAdminData([FromBody] DeleteAdminDataDto deleteData)
        {
            try
            {
                _logHelper.Info(JsonConvert.SerializeObject(deleteData));

                // 檢查權限
                if (!_tools.CheckPermission((int)Permissions.AdminDelete)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = _admin.CheckDeleteInputData(deleteData);

                if (inputVaild)
                {
                    bool result = _admin.DeleteAdminData(deleteData);

                    return Ok(result);
                }
                else
                {
                    _logHelper.Warn(JsonConvert.SerializeObject(deleteData));
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