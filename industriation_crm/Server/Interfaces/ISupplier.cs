using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface ISupplier
    {
        public List<supplier> GetSupplierDetails();
        public void AddSupplier(supplier supplier);
        public void UpdateSupplierDetails(supplier supplier);
        public supplier GetSupplierData(int id);
        public void DeleteSupplier(int id);
    }
}
