using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface ICategory
    {
        public List<category> GetCategoryDetails();
        public int AddCategory(category category);
        public void UpdateCategoryDetails(category category);
        public category GetCategoryData(int id);
    }
}
