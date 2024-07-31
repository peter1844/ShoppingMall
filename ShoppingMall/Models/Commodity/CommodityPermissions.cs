namespace ShoppingMall.Models.Commodity
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class CommodityPermissionsDtoResponse
    {
        /// <summary>
        /// 新增權限
        /// </summary>
        public bool InsertPermission { get; set; }
        /// <summary>
        /// 編輯權限
        /// </summary>
        public bool UpdatePermission { get; set; }
    }
}