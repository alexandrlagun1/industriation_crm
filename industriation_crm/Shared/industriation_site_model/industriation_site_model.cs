using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.industriation_site_model
{
    public class industriation_product
    {
        public string? model { get; set; }
        public string? manufacturer_name { get; set; }
        public string? price { get; set; }
        public string? name { get; set; }
        public int? product_id { get; set; }
        public string? main_category_id { get; set; }
        public string? img { get; set; }
        public int? quantity_class_id { get; set; }
        public int? is_clone { get; set; }
        public int? status { get; set; }
    }
    public class price_model
    {
        public int? product_id { get; set; }
        public string? price { get; set; }
    }
}
