using industriation_crm.Server.Models;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Ubiety.Dns.Core;

namespace industriation_crm.Server.Retail
{
    public static class RetailOrderCreator
    {
        static readonly HttpClient httpClient = new HttpClient();
        public static void CreateClient()
        {

        }
        public static void CreateOrder(order order)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("site", "industriation");

            string items = null;
            List<product_to_order> product_To_Orders = order.order_Checks.FirstOrDefault().product_To_Orders;
            for (int i = 0; i < product_To_Orders.Count(); i++)
            {
                product product = new product();
                using (DatabaseContext context = new DatabaseContext())
                {
                    product = context.product.Where(p => p.id == product_To_Orders[i].product_id).FirstOrDefault();
                }
                string initialPrice = product_To_Orders[i].product_price.ToString().Replace(',', '.');

                items += "{\"quantity\":" + product_To_Orders[i].count + ",\"initialPrice\":" + initialPrice + ",\"offer\":{\"externalId\":\"" + product.external_id + "\"}}";
                if (i + 1 != product_To_Orders.Count)
                    items += ",";
            }

            client client = new client();
            using (DatabaseContext context = new DatabaseContext())
            {
                client = context.client.Include(c => c.contacts).Where(c => c.id == order.client_id).FirstOrDefault();
            }
            contact contact = client.contacts.Where(c => c.main_contact == 1).FirstOrDefault();
            string order_type = "";
            string contragent_type = "";
            string delivery_type = "";

            if (order.delivery.delivery_type_id == 1)
                delivery_type = "self-delivery";
            else if (order.delivery.delivery_type_id == 4)
            {
                delivery_type = "dostavka-do-terminala-tk-za-schet-pokupatelya";
            }
            else
            {
                delivery_type = "dostavka-do-terminala-tk";
            }

            if (client.client_type == 1)
            {
                order_type = "eshop-legal";
                contragent_type = "legal-entity";
            }
            else
            {
                order_type = "eshop-individual";
                contragent_type = "individual";
            }

            string date = "";
            if (order.delivery.shipment_date != null)
                date = "\"date\":\"" + order.delivery.shipment_date.Value.ToString("yyyy-MM-dd") + "\",";

            int client_id = GetClient(client.org_inn);
            string customer = "";

            if (client_id != 0)
                customer = "{\"customer\":{\"id\":" + client_id + "},";

            dict.Add("order",
                customer + "\"firstName\":\"" + contact?.name
                + "\",\"lastName\":\"" + contact?.surname
                + "\",\"patronymic\":\"" + contact?.patronymic
                + "\",\"number\":\"" + order.id + "C"
                + "\",\"externalId\":\"" + order.id + "C"
                + "\",\"orderType\":\"" + order_type
                + "\",\"phone\":\"" + contact?.phone
                + "\",\"email\":\"" + contact?.email
                + "\",\"contragent\":{\"contragentType\":\"" + contragent_type + "\",\"INN\":\"" + client.org_inn + "\",\"OGRN\":\"" + client.org_ogrn + "\", \"KPP\": \"" + client.org_kpp + "\",\"legalName\":\"" + client?.org_name?.Replace("\"", "'") + "\", \"legalAddress\":\"" + client?.org_address + "\",\"BIK\":\"" + client?.bank_bik + "\",\"bank\":\"" + client?.bank_name + "\",\"corrAccount\":\"" + client?.bank_cor_schet + "\",\"bankAccount\":\"" + client?.bank_ras_schet + "\"}"
                + ",\"delivery\":{\"code\":\"" + delivery_type + "\"," + date + "\"address\": {\"text\":\"" + order.delivery.address + "\"}}"
                + ",\"items\":[" + items + "]"
                + "}");

            //"delivery":{"code": "dostavka-adres"}

            var req = new HttpRequestMessage(HttpMethod.Post, "https://industriation.retailcrm.ru/api/v5/orders/create") { Content = new FormUrlEncodedContent(dict) };
            req.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
            var res = httpClient.SendAsync(req).Result;
            string s = res.Content.ReadAsStringAsync().Result;

        }
        public static void UpdateOrder(order order)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("site", "industriation");

            string items = null;
            List<product_to_order> product_To_Orders = order.order_Checks.FirstOrDefault().product_To_Orders;
            for (int i = 0; i < product_To_Orders.Count(); i++)
            {
                product product = new product();
                using (DatabaseContext context = new DatabaseContext())
                {
                    product = context.product.Where(p => p.id == product_To_Orders[i].product_id).FirstOrDefault();
                }
                string initialPrice = product_To_Orders[i].product_price.ToString().Replace(',', '.');

                items += "{\"quantity\":" + product_To_Orders[i].count + ",\"initialPrice\":" + initialPrice + ",\"offer\":{\"externalId\":\"" + product.external_id + "\"}}";
                if (i + 1 != product_To_Orders.Count)
                    items += ",";
            }

            client client = new client();
            using (DatabaseContext context = new DatabaseContext())
            {
                client = context.client.Include(c => c.contacts).Where(c => c.id == order.client_id).FirstOrDefault();
            }
            contact contact = client.contacts.Where(c => c.main_contact == 1).FirstOrDefault();
            string order_type = "";
            string contragent_type = "";
            string delivery_type = "";

            if (order.delivery.delivery_type_id == 1)
                delivery_type = "self-delivery";
            else if (order.delivery.delivery_type_id == 4)
            {
                delivery_type = "dostavka-do-terminala-tk-za-schet-pokupatelya";
            }
            else
            {
                delivery_type = "dostavka-do-terminala-tk";
            }

            if (client.client_type == 1)
            {
                order_type = "eshop-legal";
                contragent_type = "legal-entity";
            }
            else
            {
                order_type = "eshop-individual";
                contragent_type = "individual";
            }

            string date = "";
            if (order.delivery.shipment_date != null)
                date = "\"date\":\"" + order.delivery.shipment_date.Value.ToString("yyyy-MM-dd") + "\",";

            int client_id = GetClient(client.org_inn);
            string customer = "";

            if (client_id != 0)
                customer = "{\"customer\":{\"id\":" + client_id + "},";

            dict.Add("order",
                customer + "\"firstName\":\"" + contact?.name
                + "\",\"lastName\":\"" + contact?.surname
                + "\",\"patronymic\":\"" + contact?.patronymic
                + "\",\"orderType\":\"" + order_type
                + "\",\"phone\":\"" + contact?.phone
                + "\",\"email\":\"" + contact?.email
                + "\",\"contragent\":{\"contragentType\":\"" + contragent_type + "\",\"INN\":\"" + client.org_inn + "\",\"OGRN\":\"" + client.org_ogrn + "\", \"KPP\": \"" + client.org_kpp + "\",\"legalName\":\"" + client?.org_name?.Replace("\"", "'") + "\", \"legalAddress\":\"" + client?.org_address + "\",\"BIK\":\"" + client?.bank_bik + "\",\"bank\":\"" + client?.bank_name + "\",\"corrAccount\":\"" + client?.bank_cor_schet + "\",\"bankAccount\":\"" + client?.bank_ras_schet + "\"}"
                + ",\"delivery\":{\"code\":\"" + delivery_type + "\"," + date + "\"address\": {\"text\":\"" + order.delivery.address + "\"}}"
                + ",\"items\":[" + items + "]"
                + "}");

            //"delivery":{"code": "dostavka-adres"}

            var req = new HttpRequestMessage(HttpMethod.Post, $"https://industriation.retailcrm.ru/api/v5/orders/{order.id}C/edit") { Content = new FormUrlEncodedContent(dict) };
            req.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
            var res = httpClient.SendAsync(req).Result;
            string s = res.Content.ReadAsStringAsync().Result;
        }
        public static void FindCreateProduct(int externalId)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.retailcrm.ru/api/v5/store/products?filter[externalId]={externalId}");
            req.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
            var res = httpClient.SendAsync(req).Result;
            var productData = res.Content.ReadFromJsonAsync<productData>().Result;

            if(productData.products.Count == 0)
            {

            }

        }
        private static int GetClient(long? inn)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.retailcrm.ru/api/v5/customers?filter[contragentInn]={inn}");
            req.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
            var res = httpClient.SendAsync(req).Result;
            var clientData = res.Content.ReadFromJsonAsync<clientData>().Result;

            if (clientData.customers != null && clientData.customers.Count != 0)
                return clientData.customers.FirstOrDefault().id;
            return 0;
        }
    }
}
