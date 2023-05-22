using HtmlAgilityPack;
using industriation_crm.Server.Models;
using industriation_crm.Shared.DaData;
using industriation_crm.Shared.Models;
using industriation_crm.Shared.RetailData;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System.IO;
using System.Net.Http;
using System.Text;
using Ubiety.Dns.Core;
using industriation_crm.Shared.industriation_site_model;
using Serilog;
using Blazorise;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.JsonPatch.Operations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace industriation_crm.Server.Retail
{
    public static class RetailOrderCreator
    {
        static string retail_login = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("RetailData")["Login"];
        static string retail_pass = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("RetailData")["Password"];
        static string retail_api_key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("RetailData")["ApiKey"];
        static readonly HttpClient httpClient = new HttpClient();

        public static async void TryRemoveProduct(industriation_product industriation_Product)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.retailcrm.ru/api/v5/store/products?filter[externalId]={industriation_Product.product_id}&site=industriation");
            req.Headers.Add("X-API-KEY", retail_api_key);
            var res = httpClient.SendAsync(req).Result;
            string json = res.Content.ReadAsStringAsync().Result;
            int? retail_product_id = null;
            try
            {
                if (!String.IsNullOrEmpty(json))
                {
                    dynamic obj = JsonConvert.DeserializeObject(json);
                    string offerId = "";
                    if (obj?.products.Count != 0)
                        retail_product_id = obj?.products[0].id;
                    else
                        return;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.retailcrm.ru/api/v5/orders?filter[product]={industriation_Product.model}&limit=100");
            req.Headers.Add("X-API-KEY", retail_api_key);
            res = httpClient.SendAsync(req).Result;
            var productData = res.Content.ReadFromJsonAsync<GetOrderResult>().Result;
            bool isDelete = true;
            foreach (var o in productData.orders)
            {
                foreach (var i in o.items)
                {
                    if (i.offer?.externalId == industriation_Product.product_id.ToString())
                        isDelete = false;
                }
            }

            if (isDelete)
            {
                try
                {
                    Authorization();
                    string AddProductJson = "[{\"operationName\":\"deleteProduct\",\"variables\":{\"input\":{\"id\":" + retail_product_id + "}},\"query\":\"mutation deleteProduct($input: DeleteProductInput!) {\\n deleteProduct(input: $input) {\\n product {\\n id\\n __typename\\n    }\\n __typename\\n  }\\n}\\n\"}]";
                    var content = new StringContent(AddProductJson, Encoding.UTF8, "application/json");
                    var result = httpClient.PostAsync("https://industriation.retailcrm.ru/app/api/batch", content).Result;
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        Log.Error("Продукт удален");
                    else
                        Log.Error($"ОШИБКА для удаления продукта {result.StatusCode}");

                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }
            else
            {
                var dict = new Dictionary<string, string>();
                string site_url = "";

                dict.Add("products",
                    "{\"products\":{\"id\":\""+retail_product_id+"\",\"externalId\":\"" + industriation_Product.product_id + "\",\"externalId\":\"" + industriation_Product.product_id + "D\"}}"
                    );
                req = new HttpRequestMessage(HttpMethod.Post, "https://industriation.retailcrm.ru/api/v5/store/products/batch/edit") { Content = new FormUrlEncodedContent(dict) };
                req.Headers.Add("X-API-KEY", retail_api_key);
                try
                {
                    res = httpClient.SendAsync(req).Result;
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
                Log.Error("Есть заказы с продуктом");
            }

        }
        public static void CheckRetailProduct(product product)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.retailcrm.ru/api/v5/store/products?filter[externalId]={product.external_id}");
            req.Headers.Add("X-API-KEY", retail_api_key);
            var res = httpClient.SendAsync(req).Result;
            string json = res.Content.ReadAsStringAsync().Result;

            try
            {
                if (!String.IsNullOrEmpty(json))
                {
                    dynamic obj = JsonConvert.DeserializeObject(json);
                    string offerId = "";
                    if (obj?.products.Count != 0)
                        UpdateProduct(product);
                    else
                        AddProduct(product);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }


        }
        public static void UpdateProduct(product product)
        {
            var dict = new Dictionary<string, string>();
            string site_url = "";
            if (!String.IsNullOrEmpty(product.site_url))
                site_url = "\"url\":\"" + product.site_url + "\",";
            string article = "";
            if (!String.IsNullOrEmpty(product.article))
                article = "\"article\":\"" + product.article + "\",";
            string name = "";
            if (!String.IsNullOrEmpty(product.name))
                name = "\"name\":\""+product.name+"\",";
            string manufacturer = "";
            if (!String.IsNullOrEmpty(product.manufacturer))
                manufacturer = "\"manufacturer\":\"" + product.manufacturer+"\",";

            dict.Add("products",
                "{\"products\":{\"externalId\":\"" + product.external_id + "\",\"groups\":[{\"id\":\"19\"}]," + site_url + "\"site\":\"industriation\", " + article + name + manufacturer +"\"externalId\":\"" + product.external_id + "\"}}"
                );
            var req = new HttpRequestMessage(HttpMethod.Post, "https://industriation.retailcrm.ru/api/v5/store/products/batch/edit") { Content = new FormUrlEncodedContent(dict) };
            req.Headers.Add("X-API-KEY", retail_api_key);
            try
            {
                var res = httpClient.SendAsync(req).Result;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
            updateProductOffer(product);
            Console.WriteLine($"{product.id} обновлен");
        }
        public static void AddProduct(product product)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("products",
                "{\"products\":{\"catalogId\": 2,\"groups\":[{\"id\":\"19\"}],\"article\":\"" + product.article + "\", \"url\":\"" + product.site_url + "\",\"name\":\"" + product.name + "\", \"externalId\":\"" + product.external_id + "\", \"manufacturer\":\"" + product.manufacturer + "\"}}"
                );
            var req = new HttpRequestMessage(HttpMethod.Post, "https://industriation.retailcrm.ru/api/v5/store/products/batch/create") { Content = new FormUrlEncodedContent(dict) };
            req.Headers.Add("X-API-KEY", retail_api_key);
            try
            {
                var res = httpClient.SendAsync(req).Result;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
            updateProductOffer(product);
            Console.WriteLine($"{product.id} добавлен");
        }
        public static void AddImage(string link, string id)
        {
            try
            {
                string AddProductJson = "[{\"operationName\":\"editProduct\",\"variables\":{\"input\":{\"id\":" + id + ",\"images\":[\"" + link + "\"]}},\"query\":\"mutation editProduct($input: EditProductInput!) {\\n  editProduct(input: $input) {\\n    product {\\n      id\\n      __typename\\n    }\\n    deactivatedOffersIds\\n    __typename\\n  }\\n}\\n\"}]";
                var content = new StringContent(AddProductJson, Encoding.UTF8, "application/json");
                var result = httpClient.PostAsync("https://industriation.retailcrm.ru/app/api/batch", content).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    Console.WriteLine($"Загружено изображение");
                else
                    Console.WriteLine($"ОШИБКА для загрузки изображения {result.StatusCode}");
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
        private static async Task<string> AddImageToRetailFtp(byte[] img, string filename)
        {

            try
            {
                if (img != null)
                {
                    MultipartFormDataContent multipartContent = new MultipartFormDataContent();
                    string request = "{\"operationName\":\"uploadImages\",\"variables\":{\"files\":[null]},\"query\":\"mutation uploadImages($files: [Upload!]!) {\\n  uploadImages(input: {files: $files}) {\\n    images {\\n      path\\n      errors\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}";
                    multipartContent.Add(new StringContent(request), "operations");
                    multipartContent.Add(new StringContent("{\"1\":[\"variables.files.0\"]}"), "map");
                    multipartContent.Add(new ByteArrayContent(img), "1", filename);

                    HttpResponseMessage responseGetXml = await httpClient.PostAsync("https://industriation.retailcrm.ru/app/api", multipartContent);
                    string json = await responseGetXml.Content.ReadAsStringAsync();

                    dynamic obj = JsonConvert.DeserializeObject(json);
                    string link = obj.data.uploadImages.images[0].path;
                    return link;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return null;
            }
        }



        private static async Task updateProductOffer(product product)
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.retailcrm.ru/api/v5/store/products?filter[externalId]={product.external_id}");
                req.Headers.Add("X-API-KEY", retail_api_key);
                var res = httpClient.SendAsync(req).Result;
                string json = res.Content.ReadAsStringAsync().Result;
                dynamic obj = JsonConvert.DeserializeObject(json);
                string offerId = "";
                if (obj?.products.Count != 0)
                    offerId = obj?.products[0]?.offers[0]?.id;
                string retailProductId = obj?.products[0]?.id;
                Authorization();

                //Обновление оффера
                string product_name = "";
                if (!String.IsNullOrEmpty(product.name))
                    product_name = $"\"name\":\"{product.name}\",";
                string query = "[{\"operationName\":\"editOffer\",\"variables\":{\"input\":{\"externalId\":\"" + product.external_id + "\"," + product_name + "\"id\":\"" + offerId + "\"}},\"query\":\"mutation editOffer($input: EditOfferInput!) {\\n  editOffer(input: $input) {\\n    offer {\\n      id\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}]";
                var content = new StringContent(query, Encoding.UTF8, "application/json");
                var result = httpClient.PostAsync("https://industriation.retailcrm.ru/app/api/batch", content).Result;
                //3 - м
                //1 - шт
                //Обновление свойств
                int unit = 1;
                if (product.unit == "м")
                    unit = 3;
                query = "[{\"operationName\":\"editProductProperties\",\"variables\":{\"input\":{\"id\":\"" + retailProductId + "\",\"unit\":\"" + unit + "\",\"article\":\"" + product.article + "\"}},\"query\":\"mutation editProductProperties($input: EditProductInput!) {\\n  editProduct(input: $input) {\\n    product {\\n      offersWithPagination(first: 1) {\\n        edges {\\n          node {\\n            properties {\\n              field {\\n                code\\n                name\\n                variative\\n                __typename\\n              }\\n              __typename\\n            }\\n            __typename\\n          }\\n          __typename\\n        }\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}]";
                content = new StringContent(query, Encoding.UTF8, "application/json");
                result = httpClient.PostAsync("https://industriation.retailcrm.ru/app/api/batch", content).Result;

                updatePrice(product.external_id, product.price.ToString());


                Console.WriteLine("Попытка получить изображение");
                req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.ru/index.php?route=tool/image_content&p_id={product.external_id}&key=2n6aKEuz6H4Avy46rhNoLat3UgCJx259&CRM=1");
                res = httpClient.SendAsync(req).Result;
                var ind_site_img = res.Content.ReadFromJsonAsync<ind_site_img>().Result;
                Console.WriteLine("Изображение получено");

                if (ind_site_img != null && ind_site_img.content != null && !String.IsNullOrEmpty(ind_site_img.name))
                {
                    Console.WriteLine("Загрузка изображения на фтп");
                    string link = AddImageToRetailFtp(ind_site_img.content, ind_site_img.name).Result;
                    if (!String.IsNullOrEmpty(link))
                    {
                        AddImage(link, retailProductId);
                        Console.WriteLine("Загрузка изображения в ретейл");
                    }
                }
            }
            catch (Exception e)
            {
                //await updateProductOffer(product);
                Log.Error(e.ToString());
                return;
            }
        }
        public static void updatePrice(int? externalId, string? price)
        {
            try
            {

                var dict = new Dictionary<string, string>();
                dict.Add("prices",
                    "{\"prices\":{\"site\":\"industriation\",\"externalId\":\"" + externalId + "\",\"prices\":[{\"price\":" + price?.Replace(',', '.') + ",\"code\":\"base\"}]}}"
                    );
                var req = new HttpRequestMessage(HttpMethod.Post, "https://industriation.retailcrm.ru/api/v5/store/prices/upload") { Content = new FormUrlEncodedContent(dict) };
                req.Headers.Add("X-API-KEY", retail_api_key);
                var res = httpClient.SendAsync(req).Result;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        public static void Authorization()
        {
            try
            {
                HttpResponseMessage response = httpClient.GetAsync("https://industriation.retailcrm.ru/").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                String h1 = GetHeaders(response);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(responseBody);
                var inputs = doc.DocumentNode.SelectNodes("//input");
                string csrf = "";
                if (inputs == null)
                {
                    return;
                }
                if (inputs.Count < 3)
                    return;
                foreach (var atr in inputs[3].Attributes)
                {
                    if (atr.Name == "value")
                        csrf = atr.Value;
                }
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(retail_login), "_username");
                form.Add(new StringContent(retail_pass), "_password");
                form.Add(new StringContent(csrf), "_csrf_token");
                HttpResponseMessage responseAuth = httpClient.PostAsync("https://industriation.retailcrm.ru/login_check", form).Result;
                String h2 = GetHeaders(responseAuth);
                string resAHeaders = "";
                foreach (var header in response.Headers)
                {
                    resAHeaders += header;
                }

                response = httpClient.GetAsync("https://industriation.retailcrm.ru/").Result;
                string resHeaders = "";
                foreach (var header in response.Headers)
                {
                    resHeaders += header;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return;
            }
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
            try
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
                    + "\"customFields\":{\"op_dates\":\"" + date_postavka + "\",\"op_number_position\":\"" + positions + "\",\"sposob_oplaty\":\"" + order.retail_sposob_oplaty + "\"}"
                    + "}");

                //"delivery":{"code": "dostavka-adres"}

                var req = new HttpRequestMessage(HttpMethod.Post, "https://industriation.retailcrm.ru/api/v5/orders/create") { Content = new FormUrlEncodedContent(dict) };
                req.Headers.Add("X-API-KEY", retail_api_key);
                var res = httpClient.SendAsync(req).Result;
                string s = res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return;
            }

        }
        private static bool? CheckOrderAvailability(string externalId)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://industriation.retailcrm.ru/api/v5/orders/{externalId}?site=industriation");
            req.Headers.Add("X-API-KEY", retail_api_key);
            try
            {
                var res = httpClient.SendAsync(req).Result;
                var productData = res.Content.ReadFromJsonAsync<GetOrderResult>().Result;
                return productData.success;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return null;
            }

        }
        public static void UpdateOrder(order order)
        {
            bool? checkOrderAvailability = false;
            if (String.IsNullOrEmpty(order.retail_id))
                checkOrderAvailability = CheckOrderAvailability($"{order.id}-{order._current_check.check_number}");
            else
                checkOrderAvailability = CheckOrderAvailability($"{order.retail_id}");
            if (checkOrderAvailability == false)
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


                //Статус заказа
                string order_status = "";
                if (order.order_status_id == 1 || order.order_status_id == 3)
                {
                    if (order.pay_status_id == 4)
                        order_status = "availability-confirmed";
                    if (order.pay_status_id == 1)
                        order_status = "assembling";
                    if (order.pay_status_id == 2)
                        order_status = "send-to-assembling";
                    if (order.pay_status_id == 3)
                        order_status = "assembling-complete";
                }
                if (order.order_status_id == 7)
                    order_status = "cancel-other";
                if (order.order_status_id == 9)
                    order_status = "ready-for-otgruzhen";
                if (order.order_status_id == 10)
                    order_status = "send-to-delivery";
                if (order.order_status_id == 11)
                    order_status = "shippment-ready";
                if (order.order_status_id == 12)
                    order_status = "complete";

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

                string? shipment_date = "";
                if (order?.delivery?.shipment_date != null)
                    shipment_date = order?.delivery?.shipment_date.Value.ToString("yyyy-MM-dd");

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
                    + "\",\"status\":\"" + order_status
                    + "\",\"phone\":\"" + contact?.phone
                    + "\",\"email\":\"" + contact?.email + "\""
                    + contragent
                    + ",\"delivery\":{\"code\":\"" + delivery_type + "\"," + /*date +*/ "\"address\": {\"text\":\"" + order.delivery.address + "\"}}"

                    + ",\"items\":[" + items + "],"
                    + "\"customFields\":{\"op_dates\":\"" + date_postavka + "\",\"op_number_position\":\"" + positions + "\", \"committed_shipment_date_manual\":\"" + shipment_date + "\",\"sposob_oplaty\":\"" + order.retail_sposob_oplaty + "\"}"
                    + "}");

                //"delivery":{"code": "dostavka-adres"}
                HttpRequestMessage req = new();
                if (String.IsNullOrEmpty(order.retail_id))
                    req = new HttpRequestMessage(HttpMethod.Post, $"https://industriation.retailcrm.ru/api/v5/orders/{order.id}-{order._current_check.check_number}/edit") { Content = new FormUrlEncodedContent(dict) };
                else
                    req = new HttpRequestMessage(HttpMethod.Post, $"https://industriation.retailcrm.ru/api/v5/orders/order.retail_id/edit") { Content = new FormUrlEncodedContent(dict) };
                req.Headers.Add("X-API-KEY", retail_api_key);
                var res = httpClient.SendAsync(req).Result;
                string s = res.Content.ReadAsStringAsync().Result;

                foreach (var p in order.order_Pays.Where(p => p.is_new == true && p.isRemove == false))
                {
                    var payDict = new Dictionary<string, string>();
                    payDict.Add("site", "industriation");
                    if (String.IsNullOrEmpty(order.retail_id))
                        payDict.Add("payment", "{\"order\":{\"externalId\":\"" + order.id + "-" + order._current_check.check_number + "\"}, \"amount\":" + p.price + ", \"type\":\"bank-transfer\", \"status\":\"paid\"}");
                    else
                        payDict.Add("payment", "{\"order\":{\"externalId\":\"" + order.retail_id + "\"}, \"amount\":" + p.price + ", \"type\":\"bank-transfer\", \"status\":\"paid\"}");
                    var payreq = new HttpRequestMessage(HttpMethod.Post, $"https://industriation.retailcrm.ru/api/v5/orders/payments/create") { Content = new FormUrlEncodedContent(payDict) };
                    payreq.Headers.Add("X-API-KEY", retail_api_key);
                    var payres = httpClient.SendAsync(payreq).Result;
                    PaymentAnswer? paymentAnswer = payres.Content.ReadFromJsonAsync<PaymentAnswer>().Result;
                    p.retail_id = paymentAnswer.id;
                }
                foreach (var p in order.order_Pays.Where(p => p.retail_id != null && p.isRemove == true))
                {

                    var payreq = new HttpRequestMessage(HttpMethod.Post, $"https://industriation.retailcrm.ru/api/v5/orders/payments/{p.retail_id}/delete");
                    payreq.Headers.Add("X-API-KEY", retail_api_key);
                    httpClient.SendAsync(payreq);

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
