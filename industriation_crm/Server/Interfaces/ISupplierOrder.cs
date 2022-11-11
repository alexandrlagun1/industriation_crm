﻿using industriation_crm.Shared.Models;

namespace industriation_crm.Server.Interfaces
{
    public interface ISupplierOrder
    {
        public List<supplier_order> GetSupplierOrderDetails();
        public void AddSupplierOrder(supplier_order supplier_order);
        public void UpdateSupplierOrderDetails(supplier_order supplier_order);
        public supplier_order GetSupplierOrderData(int id);
        public void DeleteSupplierOrder(int id);
    }
}