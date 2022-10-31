using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Services
{
    public class UserManager : IUser
    {
        readonly DatabaseContext _dbContext = new();
        public UserManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<user> GetUserDetails(string trigger)
        {
            try
            {

                List<user> users = new List<user>();
                if (trigger == "all")
                    users = _dbContext.user.Include(c => c.roles).Include(c => c.clients).ToList();
                if (trigger == "managers")
                    users = _dbContext.user.Include(c => c.roles).Include(c => c.clients).Where(u => u.roles.id == 1).ToList();
                if (trigger == "suppliers")
                    users = _dbContext.user.Include(c => c.roles).Include(c => c.clients).Where(u => u.roles.id == 6).ToList();

                return users;
            }
            catch
            {
                throw;
            }
        }

        public void AddUser(user user)
        {
            try
            {
                _dbContext.user.Add(user);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public void UpdateUserDetails(user user)
        {
            try
            {
                _dbContext.Entry(user).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public user GetUserData(int id)
        {
            try
            {
                user? user = _dbContext.user.Find(id);
                if (user != null)
                {
                    return user;
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
        public void DeleteUser(int id)
        {
            try
            {
                user? user = _dbContext.user.Find(id);
                if (user != null)
                {
                    _dbContext.user.Remove(user);
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

        public user UserLogin(user _user)
        {
            try
            {
                user? user = _dbContext.user.Where(u => u.password == _user.password && u.login == _user.login).FirstOrDefault();
                if (user != null)
                {
                    return user;
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

