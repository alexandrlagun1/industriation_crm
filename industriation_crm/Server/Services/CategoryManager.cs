using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class CategoryManager : ICategory
    {
        readonly DatabaseContext _dbContext = new();
        public CategoryManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int AddCategory(category category)
        {
            throw new NotImplementedException();
        }

        public category GetCategoryData(int id)
        {
            throw new NotImplementedException();
        }

        public List<category> GetCategoryDetails()
        {
            try
            {
                List<category> categories = _dbContext.category.ToList();
                return categories;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateCategoryDetails(category category)
        {
            throw new NotImplementedException();
        }
    }
}
