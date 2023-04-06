using HtmlAgilityPack;
using industriation_crm.Server.Models;
using industriation_crm.Shared.DaData;
using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System.Net.Http;
using System.Text;
using Ubiety.Dns.Core;

namespace industriation_crm.Server.Retail
{
    public static class RetailOrderCreator
    {
        static readonly HttpClient httpClient = new HttpClient();
        public static void CreateClient()
        {

        }
        public static void AddProduct(product product)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("products",
                "{\"products\":{\"catalogId\": 2,\"article\":\""+product.article+"\", \"name\":\""+product.name+"\", \"externalId\":\""+product.external_id+"\", \"manufacturer\":\""+product.manufacturer+"\"}}"
                );
            var req = new HttpRequestMessage(HttpMethod.Post, "https://industriation.retailcrm.ru/api/v5/store/products/batch/create") { Content = new FormUrlEncodedContent(dict) };
            req.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
            var res = httpClient.SendAsync(req).Result;
            updateProductOffer(product.external_id.ToString());
        }
        private static async Task updateProductOffer(string externalId)
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.retailcrm.ru/api/v5/store/products?filter[externalId]={externalId}");
                req.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
                var res = httpClient.SendAsync(req).Result;
                string json = res.Content.ReadAsStringAsync().Result;
                dynamic obj = JsonConvert.DeserializeObject(json);
                string offerId = "";
                if (obj?.products.Count != 0)
                    offerId = obj?.products[0]?.offers[0]?.id;
                await Authorization();
                string query = "[{\"operationName\":\"editOffer\",\"variables\":{\"input\":{\"externalId\":\"" + externalId + "\",\"id\":\"" + offerId + "\"}},\"query\":\"mutation editOffer($input: EditOfferInput!) {\\n  editOffer(input: $input) {\\n    offer {\\n      id\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}]";
                var content = new StringContent(query, Encoding.UTF8, "application/json");
                var result = httpClient.PostAsync("https://industriation.retailcrm.ru/app/api/batch", content).Result;
            }
            catch (Exception e)
            {
                updateProductOffer(externalId);
                return;
            }
        }
        public static async Task Authorization()
        {
            HttpResponseMessage response = await httpClient.GetAsync("https://industriation.retailcrm.ru/");
            string responseBody = await response.Content.ReadAsStringAsync();
            String h1 = GetHeaders(response);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(responseBody);
            var inputs = doc.DocumentNode.SelectNodes("//input");
            string csrf = "";
            if (inputs == null)
            {
                await Authorization();
                return;
            }
            foreach (var atr in inputs[3].Attributes)
            {
                if (atr.Name == "value")
                    csrf = atr.Value;
            }
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent("ik@industriation.ru"), "_username");
            form.Add(new StringContent("Qwerty11"), "_password");
            form.Add(new StringContent(csrf), "_csrf_token");
            HttpResponseMessage responseAuth = await httpClient.PostAsync("https://industriation.retailcrm.ru/login_check", form);
            String h2 = GetHeaders(responseAuth);
            string resAHeaders = "";
            foreach (var header in response.Headers)
            {
                resAHeaders += header;
            }

            response = await httpClient.GetAsync("https://industriation.retailcrm.ru/");
            string resHeaders = "";
            foreach (var header in response.Headers)
            {
                resHeaders += header;
            }
            String h3 = GetHeaders(response);
            Console.WriteLine("Авторизация прошла успешно");
        }
        public static String GetHeaders(HttpResponseMessage responseAuth)
        {
            String allHeaders = Enumerable
       .Empty<(String name, String value)>()
       // Add the main Response headers as a flat list of value-tuples with potentially duplicate `name` values:
       .Concat(
           responseAuth.Headers
               .SelectMany(kvp => kvp.Value
                   .Select(v => (name: kvp.Key, value: v))
               )
       )
       // Concat with the content-specific headers as a flat list of value-tuples with potentially duplicate `name` values:
       .Concat(
           responseAuth.Content.Headers
               .SelectMany(kvp => kvp.Value
                   .Select(v => (name: kvp.Key, value: v))
               )
       )
       // Render to a string:
       .Aggregate(
           seed: new StringBuilder(),
           func: (sb, pair) => sb.Append(pair.name).Append(": ").Append(pair.value).AppendLine(),
           resultSelector: sb => sb.ToString()
       );
            return allHeaders;
        }
        public static void CreateOrder(order order)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("site", "industriation");

            string items = null;
            List<product_to_order> product_To_Orders = order._current_check.product_To_Orders;
            string positions = "";
            string date_postavka = "";
            for (int i = 0; i < product_To_Orders.Count(); i++)
            {
                product product = new product();
                using (DatabaseContext context = new DatabaseContext())
                {
                    product = context.product.Where(p => p.id == product_To_Orders[i].product_id).FirstOrDefault();
                }
                string initialPrice = product_To_Orders[i].product_price.ToString().Replace(',', '.');

                items += "{\"quantity\":" + product_To_Orders[i].count + ",\"initialPrice\":" + initialPrice + ",\"offer\":{\"externalId\":\"" + product.external_id + "\"}}";
                positions += $"{product_To_Orders[i].product_postition}";
                date_postavka += $"{product_To_Orders[i].from_delivery_period}-{product_To_Orders[i].to_delivery_period}";
                if (i + 1 != product_To_Orders.Count)
                {
                    positions += ",";
                    items += ",";
                    date_postavka += ",";
                }
                
            }
            user user = new user();
            using (DatabaseContext contex = new DatabaseContext())
            {
                user = contex.user.Where(u => u.id == order.user_id).FirstOrDefault();
            }
            string retail_user = "";
            if (user != null && user.retail_id != null)
                retail_user = $"\"managerId\":{user.retail_id},";

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

            if (client.client_type == 1 || client.client_type == 2)
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

            int client_id = 0;
            if ((client.client_type == 1 || client.client_type == 2) && client.org_inn != null)
                client_id = GetClient(client.org_inn);

            string customer = "";
            if (client_id != 0)
                customer = "\"customer\":{\"id\":" + client_id + "},";

            string contragent = "";
            if (client.client_type == 1 || client.client_type == 2)
                contragent = ",\"contragent\":{\"contragentType\":\"" + contragent_type + "\",\"INN\":\"" + client.org_inn + "\",\"OGRN\":\"" + client.org_ogrn + "\", \"KPP\": \"" + client.org_kpp + "\",\"legalName\":\"" + client?.org_name?.Replace("\"", "'") + "\", \"legalAddress\":\"" + client?.org_address?.Replace("\"", "'") + "\",\"BIK\":\"" + client?.bank_bik + "\",\"bank\":\"" + client?.bank_name?.Replace("\"", "'") + "\",\"corrAccount\":\"" + client?.bank_cor_schet + "\",\"bankAccount\":\"" + client?.bank_ras_schet + "\"}";
            dict.Add("order",
                "{"
                + customer
                + retail_user
                + "\"firstName\":\"" + contact?.name
                + "\",\"lastName\":\"" + contact?.surname
                + "\",\"patronymic\":\"" + contact?.patronymic
                + "\",\"number\":\"" + order.id + "-" + order._current_check.check_number
                + "\",\"externalId\":\"" + order.id + "-" + order._current_check.check_number
                + "\",\"orderType\":\"" + order_type
                + "\",\"phone\":\"" + contact?.phone
                + "\",\"email\":\"" + contact?.email + "\""
                + contragent
                + ",\"delivery\":{\"code\":\"" + delivery_type + "\"," + date + "\"address\": {\"text\":\"" + order.delivery.address + "\"}}"
                + ",\"items\":[" + items + "],"
                + "\"customFields\":{\"op_dates\":\""+ date_postavka + "\",\"op_number_position\":\""+ positions + "\"}"
                + "}");

            //"delivery":{"code": "dostavka-adres"}

            var req = new HttpRequestMessage(HttpMethod.Post, "https://industriation.retailcrm.ru/api/v5/orders/create") { Content = new FormUrlEncodedContent(dict) };
            req.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
            var res = httpClient.SendAsync(req).Result;
            string s = res.Content.ReadAsStringAsync().Result;

        }
        private static bool CheckOrderAvailability(string externalId)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.retailcrm.ru/api/v5/orders/{externalId}?site=industriation");
            req.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
            var res = httpClient.SendAsync(req).Result;
            var productData = res.Content.ReadFromJsonAsync<GetOrderResult>().Result;
            return productData.success;
        }
        public static void UpdateOrder(order order)
        {
            if (CheckOrderAvailability($"{order.id}-{order._current_check.check_number}") == false) 
            {
                CreateOrder(order);
            }
            else
            {
                var dict = new Dictionary<string, string>();
                dict.Add("site", "industriation");

                string items = null;
                List<product_to_order> product_To_Orders = order._current_check.product_To_Orders;
                string positions = "";
                string date_postavka = "";
                for (int i = 0; i < product_To_Orders.Count(); i++)
                {
                    product product = new product();
                    using (DatabaseContext context = new DatabaseContext())
                    {
                        product = context.product.Where(p => p.id == product_To_Orders[i].product_id).FirstOrDefault();
                    }
                    string initialPrice = product_To_Orders[i].product_price.ToString().Replace(',', '.');

                    items += "{\"quantity\":" + product_To_Orders[i].count + ",\"initialPrice\":" + initialPrice + ",\"offer\":{\"externalId\":\"" + product.external_id + "\"}}";
                    positions += $"{product_To_Orders[i].product_postition}";
                    date_postavka += $"{product_To_Orders[i].from_delivery_period}-{product_To_Orders[i].to_delivery_period}";
                    if (i + 1 != product_To_Orders.Count)
                    {
                        positions += ",";
                        items += ",";
                        date_postavka += ",";
                    }
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

                int client_id = 0;
                if ((client.client_type == 1 || client.client_type == 2) && client.org_inn != null)
                    client_id = GetClient(client.org_inn);

                string customer = "";
                if (client_id != 0)
                    customer = "\"customer\":{\"id\":" + client_id + "},";

                string contragent = "";
                if (client.client_type == 1 || client.client_type == 2)
                    contragent = ",\"contragent\":{\"contragentType\":\"" + contragent_type + "\",\"INN\":\"" + client.org_inn + "\",\"OGRN\":\"" + client.org_ogrn + "\", \"KPP\": \"" + client.org_kpp + "\",\"legalName\":\"" + client?.org_name?.Replace("\"", "'") + "\", \"legalAddress\":\"" + client?.org_address?.Replace("\"", "'") + "\",\"BIK\":\"" + client?.bank_bik + "\",\"bank\":\"" + client?.bank_name?.Replace("\"", "'") + "\",\"corrAccount\":\"" + client?.bank_cor_schet + "\",\"bankAccount\":\"" + client?.bank_ras_schet + "\"}";
                dict.Add("order",
                    "{"
                    + customer
                    + "\"firstName\":\"" + contact?.name
                    + "\",\"lastName\":\"" + contact?.surname
                    + "\",\"patronymic\":\"" + contact?.patronymic
                    + "\",\"orderType\":\"" + order_type
                    + "\",\"phone\":\"" + contact?.phone
                    + "\",\"email\":\"" + contact?.email + "\""
                    + contragent
                    + ",\"delivery\":{\"code\":\"" + delivery_type + "\"," + date + "\"address\": {\"text\":\"" + order.delivery.address + "\"}}"
                    + ",\"items\":[" + items + "],"
                    + "\"customFields\":{\"op_dates\":\"" + date_postavka + "\",\"op_number_position\":\"" + positions + "\"}"
                    + "}");

                //"delivery":{"code": "dostavka-adres"}

                var req = new HttpRequestMessage(HttpMethod.Post, $"https://industriation.retailcrm.ru/api/v5/orders/{order.id}-{order._current_check.check_number}/edit") { Content = new FormUrlEncodedContent(dict) };
                req.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
                var res = httpClient.SendAsync(req).Result;
                string s = res.Content.ReadAsStringAsync().Result;

                foreach (var p in order.order_Pays.Where(p => p.is_new == true))
                {
                    var payDict = new Dictionary<string, string>();
                    payDict.Add("site", "industriation");
                    payDict.Add("payment", "{\"order\":{\"externalId\":\"" + order.id + "-"+order._current_check.check_number+"\"}, \"amount\":" + p.price + ", \"type\":\"bank-transfer\", \"status\":\"paid\"}");

                    var payreq = new HttpRequestMessage(HttpMethod.Post, $"https://industriation.retailcrm.ru/api/v5/orders/payments/create") { Content = new FormUrlEncodedContent(payDict) };
                    payreq.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
                    var payres = httpClient.SendAsync(payreq).Result;
                }
            }
        }
        public static void FindCreateProduct(int externalId)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.retailcrm.ru/api/v5/store/products?filter[externalId]={externalId}");
            req.Headers.Add("X-API-KEY", "D4xEMlk1WsvXPdv6RnDk72n3eLkbcuXB");
            var res = httpClient.SendAsync(req).Result;
            var productData = res.Content.ReadFromJsonAsync<productData>().Result;

            if (productData.products.Count == 0)
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
