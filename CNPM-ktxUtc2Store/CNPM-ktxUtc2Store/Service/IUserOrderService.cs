namespace CNPM_ktxUtc2Store.Service
{
    public interface IUserOrderService
    {
        Task<IEnumerable<order>> UserOrders();
    }
}
