﻿using industriation_crm.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.FilterModels
{
    public class OrdersFilter
    {
        public List<int?>? managers { get; set; } = new();
        public string? client { get; set; }
        public string? product_article { get; set; }
        public string? client_email { get; set; }
        public List<int?>? order_status { get; set; } = new();
        public List<int?>? pay_status { get; set; } = new();
        public DateTime? pay_from { get; set; }
        public DateTime? order_date_from { get; set; }
        public DateTime? order_date_to { get; set; }
        public DateTime? delivey_from { get; set; }
        public DateTime? delivey_to { get; set; }
        public int order_on_page { get; set; }
        public int current_page { get; set; }
        public int? order_id { get; set; }
        public int stage { get; set; }

        public OrderFilterView filterView { get; set; } = new();
        public int user_id { get; set; }//Для сохранения и получения фильтра из бд
    }
    public class OrdersReturnData
    {
        public int count { get; set; }
        public List<order> orders { get; set; } = new();
    }
    public class OrderFilterView
    {
        public bool managers_view { get; set; } = true;
        public bool client_view { get; set; } = true;
        public bool product_article_view { get; set; } = true;
        public bool client_email_view { get; set; } = true;
        public bool order_status_view { get; set; } = true;
        public bool pay_status_view { get; set; } = true;
        public bool pay_from_view { get; set; } = true;
        public bool order_date_from_view { get; set; } = true;
        public bool order_date_to_view { get; set; } = true;
        public bool delivey_from_view { get; set; } = true;
        public bool delivey_to_view { get; set; } = true;
        public bool order_id_view { get; set; } = true;

    }
}
