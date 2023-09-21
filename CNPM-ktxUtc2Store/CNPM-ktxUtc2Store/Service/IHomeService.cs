namespace CNPM_ktxUtc2Store.Repository
{
    public interface IHomeService
    {
         List<product> GetProduct(/*string sTerm = "", int categoryId = 0*/);
       List<category> Categories();
    }
}
