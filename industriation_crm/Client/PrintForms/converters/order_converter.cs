
using industriation_crm.Shared.Models;
using System.Text.RegularExpressions;

namespace industriation_crm.Client.PrintForms
{
    public static class order_converter
    {
        public static order_print_form ConvertOrderPrintForm(order? order, string pch)
        {
            order_print_form order_Print_From = new order_print_form();

            order_Print_From.pch = pch;

            string order_date = $"{order?.order_date?.ToString("dd")} {order.order_date?.ToString("MMMM")} {order.order_date?.ToString("yyyy")}";
            string order_date_dote = $"{order.order_date?.ToString("dd")}.{order.order_date?.ToString("MM")}.{order.order_date?.ToString("yyyy")}";

            order_Print_From.order_data.order_date = order_date;
            order_Print_From.order_data.order_date_dote = order_date_dote;
            if (order.pay_conditions == 1)
                order_Print_From.order_data.payment_method = $"{order.pay_predoplata_percent}% предоплаты";
            if (order.pay_conditions == 2)
                order_Print_From.order_data.payment_method = "Постоплата";
            order_Print_From.order_data.shipping_method = order.delivery?.delivery_type?.name;
            order_Print_From.order_data.user_name = order.client?.contacts?.Where(c => c.main_contact == 1).FirstOrDefault()?.full_name;
            order_Print_From.order_data.driving_directions = "https://industriation.ru/image/catalog/sklad-map/zrelishnaya.jpg";

            order_Print_From.order_data.simpla.inn = order.client?.org_inn.ToString();
            order_Print_From.order_data.simpla.kpp = order.client?.org_kpp.ToString();
            order_Print_From.order_data.simpla.address = order.client?.org_address;
            order_Print_From.order_data.simpla.company = order.client?.org_name;

            foreach (var p in order.product_To_Orders!)
            {
                product product = new product();
                product.name = p.product?.name;
                product.quantity = p.count.ToString();
                product.model = p.product?.article;
                product.price = p._product_price_with_discount.ToString();
                product.total = p._total_price.ToString();

                if (p.delivery_period_type_id == 1)
                {
                    product.dDate = $"Срок доставки: {p.delivery_period} (рабочий день)";
                    product.dDateFull = $"Срок доставки: {p.delivery_period} (рабочий день)";
                }
                if (p.delivery_period_type_id == 2)
                {
                    product.dDate = $"Срок доставки: {p.delivery_period} (рабочая неделя)";
                    product.dDateFull = $"Срок доставки: {p.delivery_period} (рабочая неделя)";
                }
                if (p.delivery_period_type_id == 3)
                {
                    product.dDate = $"Срок доставки: {p.delivery_period} (рабочий месяц)";
                    product.dDateFull = $"Срок доставки: {p.delivery_period} (рабочий месяц)";
                }
                product.unit = p.product?.unit;
                order_Print_From.order_data.products.Add(product);
            }

            total_info sub_total = new total_info();
            sub_total.order_id = order.id.ToString();
            sub_total.code = "sub_total";
            sub_total.title = "Сумма без НДС";
            if (order._total_price != null)
            {
                sub_total.value_total = (order._total_price.Value / 1.2).ToString("0.00");
                sub_total.value = (order._total_price.Value / 1.2).ToString("0");
            }
            sub_total.sort_order = "1";
            order_Print_From.order_data.totals_info.Add(sub_total);

            total_info shipping = new total_info();
            shipping.order_id = order.id.ToString();
            shipping.code = "shipping";
            shipping.title = "Самовывоз";
            if (order.delivery?._price != null)
            {
                shipping.value_total = (order.delivery._price.Value / 1.2).ToString("0.00");
                shipping.value = (order.delivery._price.Value / 1.2).ToString("0");
            }
            shipping.sort_order = "2";
            order_Print_From.order_data.totals_info.Add(shipping);

            total_info tax = new total_info();
            tax.order_id = order.id.ToString();
            tax.code = "tax";
            tax.title = "НДС 20%";
            if (order.products_with_delivery_price != null)
            {
                tax.value = (order.products_with_delivery_price.Value - order.products_with_delivery_price.Value / 1.2).ToString("0");
                tax.value_total = (order.products_with_delivery_price.Value - order.products_with_delivery_price.Value / 1.2).ToString("0.00");
            }
            tax.sort_order = "3";
            order_Print_From.order_data.totals_info.Add(tax);

            total_info total = new total_info();
            total.order_id = order.id.ToString();
            total.code = "total";
            total.title = "Итого с НДС";
            if (order.products_with_delivery_price != null)
            {
                total.value = order.products_with_delivery_price.Value.ToString("0");
                total.value_total = order.products_with_delivery_price.Value.ToString("0.00");
                order_Print_From.summ = order.products_with_delivery_price.Value.ToString("0");
            }
            total.sort_order = "9";
            order_Print_From.order_data.totals_info.Add(total);

            return order_Print_From;
        }
    }
}
