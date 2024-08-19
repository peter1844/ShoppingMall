namespace ShoppingMall.Domain.Repository
{
    public interface IAdminRepository
    {
        (Exception, bool) Set();
    }
}
