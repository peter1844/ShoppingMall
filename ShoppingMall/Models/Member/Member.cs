namespace ShoppingMall.Models.Member
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class MemberDataDtoResponse
    {
        /// <summary>
        /// 會員ID
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
        /// 等級
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int Enabled { get; set; }
    }

    /// <summary>
    /// 編輯會員傳入資料
    /// </summary>
    public class UpdateMemberDataDto
    {
        /// <summary>
        /// 會員ID
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// 等級
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int Enabled { get; set; }
    }
}