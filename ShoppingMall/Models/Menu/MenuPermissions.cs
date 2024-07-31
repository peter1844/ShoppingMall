namespace ShoppingMall.Models.Menu
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class MenuPermissionsDtoResponse
    {
        /// <summary>
        /// 會員頁面權限
        /// </summary>
        public bool MemberPermission { get; set; }
        /// <summary>
        /// 商品頁面權限
        /// </summary>
        public bool CommodityPermission { get; set; }
        /// <summary>
        /// 訂單頁面權限
        /// </summary>
        public bool OrderPermission { get; set; }
        /// <summary>
        /// 管理者帳號頁面權限
        /// </summary>
        public bool AdminPermission { get; set; }
    }
}