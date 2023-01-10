using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.Models
{
    public class megafon_info
    {
        public string? link { get; set; }
        public string? phone { get; set; }
        public string? cmd { get; set; }
        public string? crm_token { get; set; }
        public string? callid { get; set; }
        public string? diversion { get; set; }
        public string? type { get; set; }
        public string? user { get; set; }
        public string? ext { get; set; }
        public string? telnum { get; set; }
        public string? direction { get; set; }
        public string? duration { get; set; }
        public string? status { get; set; }
        public DateTime? date_time { get; set; }
        public contact? contact { get; set; }
    }
}
