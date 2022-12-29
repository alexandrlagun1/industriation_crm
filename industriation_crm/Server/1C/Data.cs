namespace industriation_crm.Server._1C
{
    public class _1CContragent
    {
        public string? name_in_programm { get; set; }
        public string? inn { get; set; }
        public string? kpp { get; set; }
        public string? ogrn { get; set; }
        public string? bik { get; set; }
        public string? ras_schet { get; set; }

    }
    public class _1CProduct
    {
        public string? name { get; set; }
        public string? article { get; set; }
        public string? count { get; set; }
        public string? price { get; set; }
        public string? summ { get; set; }
        public dynamic? reference1c { get; set; }
    }
    public class _1COrderPay
    {
        public string? id { get; set; }
        public _1CContragent? contragent { get; set; }
        public List<_1CProduct>? products { get; set; }
    }
    public class _1CSupplierOrder
    {
        public string? id { get; set; }
        public _1CContragent? contragent { get; set; }
        public List<_1CProduct>? products { get; set; }
    }
}
