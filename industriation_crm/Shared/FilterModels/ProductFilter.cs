using industriation_crm.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.FilterModels
{
    public class ProductReturnData
    {
        public List<product> products { get; set; }
        public int count { get; set;}
    }
    public class ProductFilter
    {
        public string? name { get; set; } = "";
        public string? article { get; set; } = "";
        public double? price_from { get; set; } = 0;
        public double? price_to { get; set; } = 10000000;
        public int current_page { get; set; } 
        public int product_on_page { get; set; }
        public int category_id { get; set; }
    }
}
