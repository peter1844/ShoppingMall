using System.Collections.Generic;

namespace ShoppingMall.Models.Admin
{
    /// <summary>
    /// 管理者資料
    /// </summary>
    public class AdminUserDataDtoResponse
    {
        /// <summary>
        /// 管理者ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 帳號
        /// </summary>
        public string Acc { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public int Enabled { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public List<AdminUserRoleData> Role { get; set; }
    }

    /// <summary>
    /// 角色
    /// </summary>
    public class AdminUserRoleData
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
    }

    /// <summary>
    /// 新增管理者傳入資料
    /// </summary>
    public class InsertAdminDataDto
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 帳號
        /// </summary>
        public string Acc { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public List<int> Roles { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public int Enabled { get; set; }
    }

    /// <summary>
    /// 編輯管理者傳入資料
    /// </summary>
    public class UpdateAdminDataDto
    {
        /// <summary>
        /// 管理者ID
        /// </summary>
        public int AdminId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public List<int> Roles { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public int Enabled { get; set; }
    }

    /// <summary>
    /// 刪除管理者傳入資料
    /// </summary>
    public class DeleteAdminDataDto
    {
        /// <summary>
        /// 管理者ID
        /// </summary>
        public int AdminId { get; set; }
    }
}