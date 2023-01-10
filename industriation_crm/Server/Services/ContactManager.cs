using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace industriation_crm.Server.Services
{
    public class ContactManager : IContact
    {
        readonly DatabaseContext _dbContext = new();
        public ContactManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public contact AddContact(contact contact)
        {
            contact.full_name = $"{contact.surname} {contact.name} {contact.patronymic}";
            try
            {
                contact.is_active = 1;
                _dbContext.contact.Add(contact);

                _dbContext.SaveChanges();
                return contact;
            }
            catch
            {
                throw;
            }
        }

        public contact GetContactData(int id)
        {
            try
            {
                contact? contact = _dbContext.contact.Include(c => c.contact_phones).Where(c => c.id == id).Include(c => c.client).FirstOrDefault();
                if (contact != null)
                {
                    return contact;
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

        public List<contact> GetContactDetails()
        {
            try
            {
                List<contact> contacts = _dbContext.contact.Include(c => c.contact_phones).ToList();
                return contacts;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateContactDetails(contact contact)
        {
            contact.full_name = $"{contact.surname} {contact.name} {contact.patronymic}";
            try
            {

                _dbContext.Entry(contact).State = EntityState.Modified;
                foreach (var phone in contact.contact_phones)
                {
                    if (phone.id != 0)
                    {
                        _dbContext.Entry(phone).State = EntityState.Modified;
                    }
                    else
                    {
                        phone.contact_id = contact.id;
                        _dbContext.contact_phone.Add(phone);
                    }
                    if (phone.isRemove == true)
                    {
                        _dbContext.contact_phone.Remove(phone);
                    }
                }
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public void DeleteContact(int id)
        {
            try
            {
                contact? contact = _dbContext.contact.Find(id);
                if (contact != null)
                {
                    contact.is_active = 0;
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

        public List<contact> GetNoClientContacts()
        {
            try
            {
                List<contact> contacts = _dbContext.contact.Where(c => c.client_id == null).ToList();
                return contacts;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateContacts(List<contact> contacts)
        {
            foreach (var c in contacts)
            {
                c.full_name = $"{c.surname} {c.name} {c.patronymic}";
                try
                {
                    _dbContext.Entry(c).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }

        public contact? GetContactDataByPhone(string phone)
        {
            try
            {
                contact? contact = _dbContext.contact.Where(c=>c.phone == phone).Include(c => c.client).FirstOrDefault();
                return contact;
            }
            catch
            {
                throw;
            }
        }
    }
}
