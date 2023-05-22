namespace industriation_crm.Server.Retail
{
    public class clientData
    {
        public List<customer>? customers{get;set;}
    }
    public class customer
    {
        public int id { get; set; }
    }

    public class productData
    {
        public List<_product>? products { get; set; }
    }
    public class _product
    {
        public int id { get; set; }
    }
    public class GetOrderResult
    {
        public bool success { get; set; }
        public int totalPageCount { get; set; }
        public int currentPage { get; set; }
        public List<_order>? orders { get; set; }
    }
    public class _order
    {
        public int id { get; set; }
        public List<item>? items { get; set; }
    }
    public class item
    {
        public int id { get; set; }
        public offer? offer { get; set; }
    }
    public class offer
    {
        public string? externalId { get; set; }
    }
}
