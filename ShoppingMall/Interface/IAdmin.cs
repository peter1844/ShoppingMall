using ShoppingMall.Models.Admin;
using System.Collections.Generic;

namespace ShoppingMall.Interface
{
    public interface IAdmin
    {
        /// <summary>
        /// 取得所有管理者資料
        /// </summary>
        List<AdminUserDataDtoResponse> GetAllAdminUserData();

        /// <summary>
        /// 新增管理者資料
        /// </summary>
        bool InsertAdminData(InsertAdminDataDto insertData);

        /// <summary>
        /// 編輯管理者資料
        /// </summary>
        bool UpdateAdminData(UpdateAdminDataDto updateData);

        /// <summary>
        /// 刪除管理者資料
        /// </summary>
        bool DeleteAdminData(DeleteAdminDataDto deleteData);

        /// <summary>
        /// 檢查新增管理者資料的傳入參數
        /// </summary>
        bool CheckInsertInputData(InsertAdminDataDto insertData);

        /// <summary>
        /// 檢查編輯管理者資料的傳入參數
        /// </summary>
        bool CheckUpdateInputData(UpdateAdminDataDto updateData);

        /// <summary>
        /// 檢查刪除管理者資料的傳入參數
        /// </summary>
        bool CheckDeleteInputData(DeleteAdminDataDto deleteData);

        /// <summary>
        /// 取得管理者帳號頁面所需的選項
        /// </summary>
        List<AdminOptionDataDtoResponse> GetAllAdminOptionData();
    }
}
