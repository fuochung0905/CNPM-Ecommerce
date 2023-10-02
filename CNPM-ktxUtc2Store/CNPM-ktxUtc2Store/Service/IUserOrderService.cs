using Microsoft.AspNetCore.Mvc;

namespace CNPM_ktxUtc2Store.Service
{
    public interface IUserOrderService
    {
        Task<IEnumerable<order>> UserOrders();
        //Task<int> Dathang(int productid, int quantity);
        Task<int> XoaDatHang(int productId);
    }
}
