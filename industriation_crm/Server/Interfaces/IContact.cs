using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface IContact
    {
        public List<contact> GetContactDetails();
        public contact AddContact(contact contact);
        public void UpdateContactDetails(contact contact);
        public contact GetContactData(int id);
        public contact? GetContactDataByPhone(string phone);
        public void DeleteContact(int id);
        public List<contact> GetNoClientContacts();
        public void UpdateContacts(List<contact> contacts);
    }
}
