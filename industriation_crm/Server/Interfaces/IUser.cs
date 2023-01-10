using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IUser
    {
        public List<user> GetUserDetails(string trigger);
        public void AddUser(user user);
        public void UpdateUserDetails(user user);
        public user GetUserData(int id);
        public user? GetUserDataByPhone(string phone);
        public void DeleteUser(int id);
        public user UserLogin(user user);


    }
}
