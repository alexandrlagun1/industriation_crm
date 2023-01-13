using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.DaData
{
    public class DaDataContent
    {
        public List<suggestion>? suggestions { get; set; }
    }
    public class suggestion
    {
        public string? value { get; set; } //Имя компании
        public data? data { get; set; }

    }
    public class data
    {
        public string? inn { get; set; }
        public string? ogrn { get; set; }
        public address? address { get; set; }
    }
    public class address
    {
        public string? value { get; set; }
    }

}

