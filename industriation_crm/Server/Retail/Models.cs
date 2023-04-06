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
    }
}
