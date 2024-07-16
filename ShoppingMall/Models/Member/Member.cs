namespace ShoppingMall.Models.Member
{
    public class MemberDataDtoResponse
    {
        public int Id { get; set; }
        public string Acc { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Enabled { get; set; }
    }
    public class UpdateMemberDataDto
    {
        public int MemberId { get; set; }
        public int Level { get; set; }
        public int Enabled { get; set; }
    }
}