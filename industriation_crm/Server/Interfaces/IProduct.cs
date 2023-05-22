using industriation_crm.Shared.Models;
using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Img;

namespace industriation_crm.Server.Interfaces
{
    public interface IProduct
    {
        public List<product> GetProductDetails(int categoryId);
        public void AddProduct(product product);
        public void UpdateProductDetails(product product);
        public product GetProductData(int id);
        public ProductReturnData GetFromFilter(ProductFilter productFilter);
    }
}
