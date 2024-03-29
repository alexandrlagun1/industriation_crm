﻿using industriation_crm.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.FilterModels
{
    public class ClientFilter
    {
        public string? client { get; set; } 
        public string? inn { get; set; }
        public int current_page { get; set; }
        public int client_on_page { get; set; }
        public string? client_email { get; set; }
        public string? client_phone { get; set; }
        public string? tag { get; set; }
        public int role { get; set; }
        public ClientFilterView filterView { get; set; } = new();
        public int? user_id { get; set; }
    }
    public class ClientReturnData
    {
        public int count { get; set; }
        public List<client> clients { get; set; }
    }
    public class ClientFilterView 
    {
        public bool fio_view { get; set; } = true;
        public bool inn_view { get; set; } = true;
        public bool email_view { get; set; } = true;
        public bool phone_view { get; set; } = true;
        public bool tag_view { get; set; } = true;
    }
}
