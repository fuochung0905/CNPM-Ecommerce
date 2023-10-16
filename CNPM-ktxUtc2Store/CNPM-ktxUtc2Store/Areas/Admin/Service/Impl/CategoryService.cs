using CNPM_ktxUtc2Store.Areas.Admin.Service;

namespace CNPM_ktxUtc2Store.Areas.Admin.Service.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(category category)
        {
            try
            {
                _context.categories.Add(category);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            var category = GetById(id);
            try
            {
                if (category == null)
                {
                    return false;
                }
                _context.categories.Remove(category);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IQueryable<category> GetAll()
        {
            var data = _context.categories.AsQueryable();
            return data;
        }

        public category GetById(int id)
        {
            return _context.categories.Find(id);
        }

        public bool Update(category category)
        {
            throw new NotImplementedException();
        }
    }
}
