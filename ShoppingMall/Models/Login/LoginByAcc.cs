namespace ShoppingMall.Models.Login
{
    /// <summary>
    /// 登入結果
    /// </summary>
    public class AdminUserDataDtoResponse
    {
        /// <summary>
        /// 管理者ID
        /// </summary>
        public int AdminId { get; set; }
        /// <summary>
        /// 管理者名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }
}