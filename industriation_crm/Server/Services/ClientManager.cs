﻿using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Models;
using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

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
                _dbContext.client.Add(client);
                _dbContext.SaveChanges();
                return client.id;
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
                client? client = _dbContext.client.Include(c => c.contacts).ThenInclude(c=>c.contact_phones).Include(c => c.user).Where(c => c.id == id).FirstOrDefault();
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
                if (clientFilter.role == 6) 
                {
                    ordersReturnData.count = _dbContext.client.Where(c =>c.is_supplier == 1 &&  c.org_inn.ToString().Contains(clientFilter.inn) && c.contacts.Where(co => co.main_contact == 1 && co.full_name.Contains(clientFilter.client)).FirstOrDefault() != null).Count();
                    ordersReturnData.clients = _dbContext.client.Where(c => c.is_supplier == 1 && c.org_inn.ToString().Contains(clientFilter.inn) && c.contacts.Where(co => co.main_contact == 1 && co.full_name.Contains(clientFilter.client)).FirstOrDefault() != null)
                        .Include(c => c.user).Include(c => c.orders).Include(c => c.contacts).OrderByDescending(c => c.add_date)
                        .Skip(clientFilter.client_on_page * (clientFilter.current_page - 1)).Take(clientFilter.client_on_page).ToList();
                }
                else 
                {
                    ordersReturnData.count = _dbContext.client.Where(c => c.org_inn.ToString().Contains(clientFilter.inn) && c.contacts.Where(co => co.main_contact == 1 && co.full_name.Contains(clientFilter.client)).FirstOrDefault() != null).Count();
                    ordersReturnData.clients = _dbContext.client.Where(c => c.org_inn.ToString().Contains(clientFilter.inn) && c.contacts.Where(co => co.main_contact == 1 && co.full_name.Contains(clientFilter.client)).FirstOrDefault() != null)
                        .Include(c => c.user).Include(c => c.orders).Include(c => c.contacts).OrderByDescending(c => c.add_date)
                        .Skip(clientFilter.client_on_page * (clientFilter.current_page - 1)).Take(clientFilter.client_on_page).ToList();
                }
                foreach(var c in ordersReturnData.clients)
                {
                    if(c.user != null)
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
        public void UpdateClientDetails(client client)
        {
            client.orders = null;
            client.contacts = null;

            try
            {
                _dbContext.Entry(client).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
