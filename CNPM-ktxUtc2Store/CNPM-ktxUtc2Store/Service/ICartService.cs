namespace CNPM_ktxUtc2Store.Repository
{
    public interface ICartService
    {
        Task<int> AddItem(int productId, int quantity);
        Task<int> RemoveItem(int productId);
        Task<shoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
    }
}
