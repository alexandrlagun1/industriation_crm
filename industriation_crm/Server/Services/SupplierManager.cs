using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class SupplierManager : ISupplier
    {
        readonly DatabaseContext _dbContext = new();
        public SupplierManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddSupplier(supplier supplier)
        {
            try
            {
                _dbContext.supplier.Add(supplier);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public void DeleteSupplier(int id)
        {
            try
            {
                supplier? supplier = _dbContext.supplier.Include(s => s.supplier_Orders).Where(s => s.id == id).FirstOrDefault();
                if (supplier != null)
                {
                    _dbContext.supplier.Remove(supplier);
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public supplier GetSupplierData(int id)
        {
            try
            {
                supplier? supplier = _dbContext.supplier.Where(c => c.id == id).FirstOrDefault();
                if (supplier != null)
                {
                    return supplier;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<supplier> GetSupplierDetails()
        {
            try
            {
                return _dbContext.supplier.ToList();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateSupplierDetails(supplier supplier)
        {
            try
            {
                _dbContext.Entry(supplier).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
