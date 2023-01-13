using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace industriation_crm.Shared.PrintForms.data
{
    public class order_print_from
    {
        public string? pch { get; set; }
        public order_data? order_data { get; set; } = new order_data();
        public string? summ { get; set; }
        public string? provider { get; set; } = "ООО &quot;Промышленная Автоматизация&quot;";
        public string? address { get; set; } = "344064, Ростов-на-Дону, ул. Зрелищная 4, тел.: 8 800 550-72-52";
        public string? inn { get; set; } = "7723901680";
        public string? kpp { get; set; } = "616501001";
        public string? banck { get; set; } = "ПАО СБЕРБАНК Г. МОСКВА";
        public string? bik { get; set; } = "044525225";
        public string? rs { get; set; } = "40702810340000010964";
        public string? ks { get; set; } = "30101810400000000225";
        public string? fio { get; set; } = "Отдел продаж";
        public string? phone { get; set; } = "8 800 550-72-52";
        public string? email { get; set; } = "info@industriation.ru";

    }
    public class order_data
    {
        public string? order_date { get; set; }
        public string? order_date_dote { get; set; }
        public string? payment_method { get; set; }
        public string? shipping_method { get; set; }
        public string? user_name { get; set; } //имя заказчика
        public string? driving_directions { get; set; } //при самовывозе клеит карту
        public simpla? simpla { get; set; }
        public List<product>? products { get; set; } = new List<product>();
        public List<total_info>? totals_info { get; set; } = new List<total_info>();
        public string? summ { get; set; }
    }
    public class simpla
    {
        public string? inn { get; set; }
        public string? kpp { get; set; }
        public string? address { get; set; }
        public string? company { get; set; }
    }
    public class product
    {
        public string? name { get; set; }
        public string? href { get; set; }
        public string? model { get; set; }
        public string? quantity { get; set; }
        public string? price { get; set; }
        public string? total { get; set; }
        public string? dDate { get; set; }
        public string? dDateFull { get; set; }
        public string? unit { get; set; }
    }
    public class total_info
    {
        public string? order_total_id { get; set; }
        public string? order_id { get; set; }
        public string? code { get; set; }
        public string? title { get; set; }
        public string? value { get; set; }
        public string? sort_order { get; set; }
        public string? value_total { get; set; }
    }
}      

