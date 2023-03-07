using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class ProgressClientDiscountManager : IProgressClientDiscount
    {
        readonly DatabaseContext _dbContext = new();
        public ProgressClientDiscountManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public progress_client_discount GetClientDiscount()
        {
            
            try
            {
                progress_client_discount? progress_client_discount = _dbContext.progress_client_discount.FirstOrDefault();
                if (progress_client_discount != null)
                {
                    return progress_client_discount;
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
    }
}
