namespace industriation_crm.Client.PrintForms.data
{
    public class dogovor_print_form
    {
        public string add_date { get; set; }
        public string order_id { get; set; }
        public string? pch { get; set; } = "1";
        public string? provider { get; set; } = "ООО &quot;Промышленная Автоматизация&quot;";
        public string? address { get; set; } = "344064, Ростов-на-Дону, ул. Зрелищная 4, тел.: 8 800 550-72-52";
        public string? inn { get; set; } = "7723901680";
        public string? kpp { get; set; } = "616501001";
        public string? banck { get; set; } = "ПАО СБЕРБАНК Г. МОСКВА";
        public string? bik { get; set; } = "044525225";
        public string? rs { get; set; } = "40702810340000010964";
        public string? ks { get; set; } = "30101810400000000225";
        public string? phone { get; set; } = "8 800 550-72-52";
        public string? email { get; set; } = "info@industriation.ru";
        public contragent? contragent { get; set; }
    }
    public class contragent
    {
        public string user_name { get; set; }
        public string? provider { get; set; } 
        public string? address { get; set; } 
        public string? inn { get; set; } 
        public string? kpp { get; set; } 
        public string? banck { get; set; } 
        public string? bik { get; set; } 
        public string? rs { get; set; } 
        public string? ks { get; set; } 
        public string? phone { get; set; } 
        public string? email { get; set; } 
    }
}
