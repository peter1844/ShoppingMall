using Newtonsoft.Json;
using ShoppingMall.Api.Admin;
using ShoppingMall.App_Code;
using ShoppingMall.Helper;
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
        private AdminPermissions adminPermissionsClass;
        private AdminProccess adminProccessClass;
        private AdminOption adminOptionClass;

        public AdminController()
        {
            adminPermissionsClass = new AdminPermissions();
            adminProccessClass = new AdminProccess();
            adminOptionClass = new AdminOption();
        }

        /// <summary>
        /// 取得管理者帳號頁面權限
        /// </summary>
        [Route("getAdminPermissions")]
        [HttpGet]
        public IHttpActionResult getAdminPermissions()
        {
            try
            {
                List<AdminPermissionsDtoResponse> adminPermissions = adminPermissionsClass.GetAllAdminPermissions();

                return Ok(adminPermissions);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
            }
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
                List<AdminUserDataDtoResponse> adminUserData = adminProccessClass.GetAllAdminUserData();

                return Ok(adminUserData);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
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
                List<AdminOptionDataDtoResponse> adminOptionData = adminOptionClass.GetAllAdminOptionData();

                return Ok(adminOptionData);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Warn(ex.Message);
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
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
                LogHelper.logger.Info(JsonConvert.SerializeObject(insertData));

                // 檢查權限
                if (!Tools.CheckPermission((int)Permissions.AdminInsert)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = adminProccessClass.CheckInsertInputData(insertData);

                if (inputVaild)
                {
                    bool result = adminProccessClass.InsertAdminData(insertData);

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
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
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
                LogHelper.logger.Info(JsonConvert.SerializeObject(updateData));

                // 檢查權限
                if (!Tools.CheckPermission((int)Permissions.AdminUpdate)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = adminProccessClass.CheckUpdateInputData(updateData);

                if (inputVaild)
                {
                    bool result = adminProccessClass.UpdateAdminData(updateData);

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
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
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
                LogHelper.logger.Info(JsonConvert.SerializeObject(deleteData));

                // 檢查權限
                if (!Tools.CheckPermission((int)Permissions.AdminDelete)) return Ok(new ExceptionData { ErrorMessage = StateCode.NoPermission.ToString() });

                bool inputVaild = adminProccessClass.CheckDeleteInputData(deleteData);

                if (inputVaild)
                {
                    bool result = adminProccessClass.DeleteAdminData(deleteData);

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
                return Ok(new ExceptionData { ErrorMessage = Tools.ReturnExceptionMessage(ex.Message) });
            }
        }
    }
}