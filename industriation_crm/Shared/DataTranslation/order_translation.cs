using industriation_crm.Shared.Models;

namespace industriation_crm.Server.DataTranslation
{
    public static class order_translation
    {
        public static Dictionary<Guid, order> orders_data { get; set; } = new Dictionary<Guid, order>();
    }
}
