using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.FilterModels
{
    public class ProductFilter
    {
        public string? name { get; set; } = "";
        public string? article { get; set; } = "";
        public double? price_from { get; set; } = 0;
        public double? price_to { get; set; } = 10000000;
    }
}
