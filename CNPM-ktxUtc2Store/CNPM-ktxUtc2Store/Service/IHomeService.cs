namespace CNPM_ktxUtc2Store.Repository
{
    public interface IHomeService
    {
         Task<IEnumerable<product>> GetProduct(string sTerm = "", int categoryId = 0);
        Task<IEnumerable<category>> Categories();
    }
}
