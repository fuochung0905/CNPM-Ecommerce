namespace CNPM_ktxUtc2Store.Repository
{
    public interface ICartService
    {
        Task<int> AddItem(int productId, int quantity, string color, string size);
        Task<int> RemoveItem(int productId);
        Task<shoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        //Task<bool> Docheckout();
    }

}
