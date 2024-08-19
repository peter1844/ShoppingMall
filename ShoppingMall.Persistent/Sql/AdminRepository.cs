using ShoppingMall.Domain.Repository;

namespace ShoppingMall.Persistent.Sql
{
    public class AdminRepository : IAdminRepository
    {
        private string conn;

        public AdminRepository(string conn)
        {
            this.conn = conn;
        }

        public void Get()
        {
        }
    }
}