using industriation_crm.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.FilterModels
{
    public class SupplierOrderReturnData
    {
        public List<supplier_order> supplier_orders { get; set; } = new();
        public int count { get; set; }
    }
    public class SupplierOrderFilter
    {
        public string? supplier { get; set; } = "";
        public string? status = "";
        public DateTime? order_date_from { get; set; }
        public DateTime? order_date_to { get; set; }
        public int order_on_page { get; set; }
        public int current_page { get; set; }
        public int? supplier_order_id { get; set; }
    }
}
