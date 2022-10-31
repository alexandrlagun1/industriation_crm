using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IProductToOrder
    {
        public List<product_to_order> GetProductToOrdertDetails(int order_id);
        public product_to_order AddProductToOrder(product_to_order product_to_order);
        public void UpdateProductsToOrder(List<product_to_order> product_to_orders);
        public void DeleteProductToOrder(int id);
        public List<product_to_order> GetProductsOfSupplierOrder(int suplier_order_id);
        public List<product_to_order> GetProductsOfNoSupplierOrder();
        public void DeleteProductToOrderInRange(List<product_to_order> product_to_orders);
        public void AddProductToOrderInRange(List<product_to_order> product_to_orders);


    }
}

