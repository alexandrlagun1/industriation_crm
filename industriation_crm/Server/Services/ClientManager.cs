using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace industriation_crm.Server.Services
{
    public class ClientManager : IClient
    {
        readonly DatabaseContext _dbContext = new();
        public ClientManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddClient(client client)
        {
            try
            {
                if (client.org_inn != null)
                {
                    client? _client = _dbContext.client.Where(c => c.org_inn == client.org_inn).FirstOrDefault();
                    if (_client == null)
                    {
                        _dbContext.client.Add(client);
                        _dbContext.SaveChanges();
                        return client.id;
                    }
                    else
                        return 0;
                }
                else
                {
                    _dbContext.client.Add(client);
                    _dbContext.SaveChanges();
                    return client.id;
                }
            }
            catch
            {
                throw;
            }
        }

        public client GetClientData(int id)
        {
            try
            {
                client? client = _dbContext.client.Include(c => c.contacts).ThenInclude(c => c.contact_phones).Include(c => c.user).Where(c => c.id == id).FirstOrDefault();
                if (client != null)
                {
                    return client;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ClientReturnData GetClientDetails(ClientFilter clientFilter)
        {
            ClientReturnData ordersReturnData = new ClientReturnData();
            try
            {
                var query = _dbContext.client.Where(c =>  c.id != 0);
                if(!String.IsNullOrEmpty(clientFilter.client))
                    query = query.Where(c => c.contacts.Where(co => co.main_contact == 1 && co.full_name.Contains(clientFilter.client)).FirstOrDefault() != null);
                if (!String.IsNullOrEmpty(clientFilter.inn))
                    query = query.Where(c => c.org_inn.ToString().Contains(clientFilter.inn));
                if (clientFilter.role == 6)
                    query = query.Where(c => c.is_supplier == 1);
                if(!String.IsNullOrEmpty(clientFilter.client_email))
                    query = query.Where(c => c.contacts.Select(c => c.email).Contains(clientFilter.client_email));
                if (!String.IsNullOrEmpty(clientFilter.client_phone))
                    query = query.Where(c => c.contacts.Select(c => c.phone).Contains(clientFilter.client_phone));
                if (!String.IsNullOrEmpty(clientFilter.tag))
                    query = query.Where(c => c.tag.Contains(clientFilter.tag));

                ordersReturnData.count = query.Count();
                ordersReturnData.clients = query.Include(c => c.user).Include(c => c.orders).Include(c => c.contacts).OrderByDescending(c => c.add_date)
                        .Skip(clientFilter.client_on_page * (clientFilter.current_page - 1)).Take(clientFilter.client_on_page).ToList();

                foreach (var c in ordersReturnData.clients)
                {
                    if (c.user != null)
                        c.user!.clients = null;
                    c.orders = null;
                }
                return ordersReturnData;
            }
            catch
            {
                throw;
            }
        }
        public void DeleteClient(int id)
        {
            try
            {
                client? client = _dbContext.client.Include(c => c.contacts).Include(c => c.orders).Where(c => c.id == id).FirstOrDefault();
                if (client != null)
                {
                    client.contacts.ForEach(c => c.main_contact = null);
                    _dbContext.client.Remove(client);
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
        public string UpdateClientDetails(client client)
        {
            client.orders = null;
            client.contacts = null;

            try
            {
                var _clients = _dbContext.client.Where(c => c.org_inn == client.org_inn).ToList();
                foreach(var c in _clients)
                {
                    if (c.id != client.id)
                        return "Клиент с таким ИНН уже существует!";
                }
                _dbContext.Entry(client).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}
