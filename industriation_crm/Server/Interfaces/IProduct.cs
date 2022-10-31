using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IProduct
    {
        public List<product> GetProductDetails();
        public void AddProduct(product product);
        public void UpdateProductDetails(product product);
        public product GetProductData(int id);

    }
}
