namespace ShoppingMall.Models.Admin
{
    /// <summary>
    /// 管理者頁面選項
    /// </summary>
    public class AdminOptionDataDtoResponse
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string RoleName { get; set; }
    }
}