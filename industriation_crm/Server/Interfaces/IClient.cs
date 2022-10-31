using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IClient
    {
        public List<client> GetClientDetails();
        public int AddClient(client client);
        public void UpdateClientDetails(client client);
        public client GetClientData(int id);
        public void DeleteClient(int id);
    }
}
