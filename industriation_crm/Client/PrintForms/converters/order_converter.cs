
using industriation_crm.Shared.Models;
using System.Text.RegularExpressions;

namespace industriation_crm.Client.PrintForms
{
    public static class order_converter
    {
        public static order_print_form ConvertOrderPrintForm(order? order, string pch)
        {
            order_print_form order_Print_From = new order_print_form();
            order_Print_From.order_id = $"{order.id.ToString()}-{order._current_check.check_number}";
            order_Print_From.pch = pch;

            string order_date = $"{order?.order_date?.ToString("dd")} {order?.order_date?.ToString("MMMM")} {order?.order_date?.ToString("yyyy")}";
            string order_date_dote = $"{order?.order_date?.ToString("dd")}.{order?.order_date?.ToString("MM")}.{order?.order_date?.ToString("yyyy")}";

            order_Print_From.order_data.order_date = order_date;
            order_Print_From.order_data.order_date_dote = order_date_dote;
            if (order.pay_conditions == 1)
                order_Print_From.order_data.payment_method = $"{order.pay_predoplata_percent}% предоплаты";
            if (order.pay_conditions == 2)
                order_Print_From.order_data.payment_method = "Постоплата";
            order_Print_From.order_data.shipping_method = order.delivery?.delivery_type?.name;
            order_Print_From.order_data.user_name = order.client?.contacts?.Where(c => c.main_contact == 1).FirstOrDefault()?.full_name;
            if (order.delivery.delivery_type_id == 1)
            {
                order_Print_From.order_data.driving_directions = "https://industriation.ru/image/catalog/sklad-map/zrelishnaya.jpg";
            }
            else
            {
                order_Print_From.order_data.driving_directions = "";
            }
            order_Print_From.order_data.simpla.inn = order.client?.org_inn.ToString();
            order_Print_From.order_data.simpla.kpp = order.client?.org_kpp.ToString();
            order_Print_From.order_data.simpla.address = order.client?.org_address;
            order_Print_From.order_data.simpla.company = order.client?.org_name;

            foreach (var p in order?._current_check?.product_To_Orders!.OrderBy(p => p.product_postition))
            {
                product product = new product();
                product.name = p.product?.name;
                product.quantity = p.count.ToString();
                product.model = p.product?.article;
                product.price = (p._product_price_with_discount / 1.2).Value.ToString("0.00");
                product.total = (p._total_price / 1.2).Value.ToString("0.00");

                if (p.delivery_period_type_id == 1)
                {
                    product.dDate = $"Срок доставки: {p.from_delivery_period}-{p.to_delivery_period} (рабочий день)";
                    product.dDateFull = $"Срок доставки: {p.from_delivery_period}-{p.to_delivery_period} (рабочий день)";
                }
                if (p.delivery_period_type_id == 2)
                {
                    product.dDate = $"Срок доставки: {p.from_delivery_period}-{p.to_delivery_period} (рабочая неделя)";
                    product.dDateFull = $"Срок доставки: {p.from_delivery_period}-{p.to_delivery_period} (рабочая неделя)";
                }
                if (p.delivery_period_type_id == 3)
                {
                    product.dDate = $"Срок доставки: {p.from_delivery_period}-{p.to_delivery_period} (рабочий месяц)";
                    product.dDateFull = $"Срок доставки: {p.from_delivery_period}-{p.to_delivery_period} (рабочий месяц)";
                }
                product.unit = p.product?.unit;
                order_Print_From.order_data.products.Add(product);
            }

            total_info sub_total = new total_info();
            sub_total.order_id = order.id.ToString();
            sub_total.code = "sub_total";
            sub_total.title = "Сумма без НДС";
            if (order?._current_check?._total_product_price != null)
            {
                sub_total.value_total = (order?._current_check?._total_product_price.Value / 1.2)!.Value.ToString("0"); 
                sub_total.value = (order?._current_check?._total_product_price.Value / 1.2)!.Value.ToString("0.00");
            }
            sub_total.sort_order = "1";
            order_Print_From.order_data.totals_info.Add(sub_total);

            total_info shipping = new total_info();
            shipping.order_id = order.id.ToString();
            shipping.code = "shipping";
            shipping.title = "Самовывоз";
            if (order.delivery?._price != null)
            {
                shipping.value_total =(order.delivery._price.Value / 1.2).ToString("0");
                shipping.value = (order.delivery._price.Value / 1.2).ToString("0.00");
            }
            shipping.sort_order = "2";
            order_Print_From.order_data.totals_info.Add(shipping);

            total_info tax = new total_info();
            tax.order_id = order.id.ToString();
            tax.code = "tax";
            tax.title = "НДС 20%";
            double? products_with_delivery_price = order?._current_check._total_product_price + order?.delivery?._price;
            if (products_with_delivery_price != null)
            {
                tax.value = (products_with_delivery_price.Value - products_with_delivery_price.Value / 1.2).ToString("0.00");
                tax.value_total =  (products_with_delivery_price.Value - products_with_delivery_price.Value / 1.2).ToString("0");
            }
            tax.sort_order = "3";
            order_Print_From.order_data.totals_info.Add(tax);

            total_info total = new total_info();
            total.order_id = order.id.ToString();
            total.code = "total";
            total.title = "Итого с НДС";
            if (products_with_delivery_price != null)
            {
                total.value = products_with_delivery_price.Value.ToString("0.00");
                total.value_total =  products_with_delivery_price.Value.ToString("0");
                order_Print_From.summ = products_with_delivery_price.Value.ToString("0.00");
            }
            total.sort_order = "9";
            order_Print_From.order_data.totals_info.Add(total);

            return order_Print_From;
        }
    }
}
