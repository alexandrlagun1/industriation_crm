using industriation_crm.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.FilterModels
{
    public class ClientFilter
    {
        public string? client { get; set; } = "";
        public string? inn { get; set; } = "";
        public int current_page { get; set; }
        public int client_on_page { get; set; }
    }
    public class ClientReturnData
    {
        public int count { get; set; }
        public List<client> clients { get; set; }
    }
}
