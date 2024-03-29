﻿using industriation_crm.Shared.Models;
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
        public string? name { get; set; }
        public string? article { get; set; }
        public double? price_from { get; set; }
        public double? price_to { get; set; }
        public int current_page { get; set; }
        public int product_on_page { get; set; }
        public int category_id { get; set; }
        public List<int?>? child_categories { get; set; }

        public ProductFilterView filterView { get; set; } = new();
        public int user_id { get; set; }
    }
    public class ProductFilterView
    {
        public bool name_view { get; set; } = true;
        public bool article_view { get; set; } = true;
        public bool price_from_view { get; set; } = true;
        public bool price_to_view { get; set; } = true;
    }
}
