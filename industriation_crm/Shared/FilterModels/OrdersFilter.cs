using industriation_crm.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.FilterModels
{
    public class OrdersFilter
    {
        public List<user?>? managers { get; set; } = new();
        public string? client { get; set; } = "";
        public DateTime? order_date_from { get; set; }
        public DateTime? order_date_to { get; set; }
        public int order_on_page { get; set; }
        public int current_page { get; set; }
        public string? order_id { get; set; }
    }
    public class OrdersReturnData
    {
        public int count { get; set; }
        public List<order> orders { get; set; } = new();
    }
}
