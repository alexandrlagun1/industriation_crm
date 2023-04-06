using industriation_crm.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.FilterModels
{
    public class CallHistoryReturnData
    {
        public int count { get; set; }
        public List<call_history> call_historyes { get; set; } = new();
    }
    public class CallHistoryFilter
    {
        public string? type { get; set; }
        public List<int?>? managers { get; set; } = new();
        public DateTime? call_date_from { get; set; }
        public DateTime? call_date_to { get; set; }
        public int calls_on_page { get; set; }
        public int current_page { get; set; }
        public string? phone { get; set; }

        public CallHistoryFilterView filterView { get; set; } = new();
        public int user_id { get; set; }
    }
    public class CallHistoryFilterView
    {
        public bool type_view { get; set; } = true;
        public bool managers_view { get; set; } = true;
        public bool call_date_from_view { get; set; } = true;
        public bool call_date_to_view { get; set; } = true;
        public bool phone_view { get; set; } = true;
    }
}
