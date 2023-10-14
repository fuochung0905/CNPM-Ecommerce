namespace CNPM_ktxUtc2Store.Areas.Admin.Service
{
    public interface ICategoryService
    {
        bool Add(category category);
        bool Update(category category);
        category GetById(int id);
        bool Delete(int id);
        IQueryable<category> GetAll();
    }
}
