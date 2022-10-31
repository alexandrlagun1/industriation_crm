using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Services
{
    public class RolesManager : IRoles
    {
        readonly DatabaseContext _dbContext = new();
        public RolesManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<roles> GetRolesDetails()
        {
            try
            {
                return _dbContext.roles.ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}
