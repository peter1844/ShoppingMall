namespace ShoppingMall.Models.Admin
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class AdminPermissionsDtoResponse
    {
        /// <summary>
        /// 新增權限
        /// </summary>
        public bool InsertPermission { get; set; }
        /// <summary>
        /// 編輯權限
        /// </summary>
        public bool UpdatePermission { get; set; }
        /// <summary>
        /// 刪除權限
        /// </summary>
        public bool DeletePermission { get; set; }
    }
}