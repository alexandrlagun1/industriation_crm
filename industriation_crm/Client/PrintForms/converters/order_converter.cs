
using industriation_crm.Shared.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using industriation_crm.NumberMask;
namespace industriation_crm.Client.PrintForms
{
    public static class order_converter
    {
        public static order_print_form ConvertOrderPrintForm(order? order, string pch, int schet_type)
        {
            order_print_form order_Print_From = new order_print_form();
            order_Print_From.order_id = $"{order.id.ToString()}-{order._current_check.check_number}";
            order_Print_From.pch = pch;
            order_Print_From.fio = $"<b>{order.user?.job_title}</b><br>{order.user?.name}";
            if (!String.IsNullOrEmpty(order.user?.phone))
                order_Print_From.phone = industriation_crm.Masks.PhoneMask.GetNumber(order.user?.phone);
            if (!String.IsNullOrEmpty(order?.user?.email))
                order_Print_From.email = order.user.email;
            if(schet_type == 2)
            {
                order_Print_From.banck = "ФИЛИАЛ \"РОСТОВСКИЙ\" АО \"АЛЬФА-БАНК\"";
                order_Print_From.bik = "046015207";
                order_Print_From.ks = "30101810500000000207";
                order_Print_From.rs = "40702810726080002059";
            }
            if(schet_type == 3)
            {
                order_Print_From.banck = "ФИЛИАЛ \"ЦЕНТРАЛЬНЫЙ\" БАНКА ВТБ (ПАО)\r\n";
                order_Print_From.bik = "044525411";
                order_Print_From.ks = "30101810145250000411";
                order_Print_From.rs = "40702810507240993243";
            }
            string order_date = $"{order?.order_date?.ToString("dd")} {order?.order_date?.ToString("MMMM")} {order?.order_date?.ToString("yyyy")}";
            string order_date_dote = $"{order?.order_date?.ToString("dd")}.{order?.order_date?.ToString("MM")}.{order?.order_date?.ToString("yyyy")}";

            order_Print_From.order_data.order_date = order_date;
            order_Print_From.order_data.order_date_dote = order_date_dote;
            if (order.pay_conditions == 1)
                order_Print_From.order_data.payment_method = $"100% предоплаты";
            if (order.pay_conditions == 2)
                order_Print_From.order_data.payment_method = order.postoplata_condition;
            if (order.pay_conditions == 3)
            {
                if(order.pay_predoplata_percent != null && order.pay_predoplata_percent != 0 && !String.IsNullOrEmpty(order.pay_predoplata_condition))
                {
                    order_Print_From.order_data.payment_method += $"{order.pay_predoplata_percent}% ";
                    order_Print_From.order_data.payment_method += $"{order.pay_predoplata_condition}<br>";
                }
                if (order.second_pay_predoplata_percent != null && order.second_pay_predoplata_percent != 0 && !String.IsNullOrEmpty(order.second_pay_predoplata_condition))
                {
                    order_Print_From.order_data.payment_method += $"{order.second_pay_predoplata_percent}% ";
                    order_Print_From.order_data.payment_method += $"{order.second_pay_predoplata_condition}";
                }
            }
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
            if (order?._current_check.currency == "$")
                order_Print_From.currency = "долларов";
            else if (order?._current_check.currency == "€")
                order_Print_From.currency = "евро";
            else
                order_Print_From.currency = "рублей";

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
                product.price = (p._product_price_with_discount / 1.2).Value.ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());
                product.total = (p._total_price / 1.2).Value.ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());

                if (p.delivery_period_type_id == 1)
                {
                    product.dDate = $"Срок поставки: {p.from_delivery_period}-{p.to_delivery_period} {industriation_crm.Shared.DaysMonthyearsConverter.GetDaysFormat(p.to_delivery_period)}";
                    product.dDateFull = $"Срок поставки: {p.from_delivery_period}-{p.to_delivery_period} {industriation_crm.Shared.DaysMonthyearsConverter.GetDaysFormat(p.to_delivery_period)}";
                }
                if (p.delivery_period_type_id == 2)
                {
                    product.dDate = $"Срок поставки: {p.from_delivery_period}-{p.to_delivery_period} {industriation_crm.Shared.DaysMonthyearsConverter.GetWeekFormat(p.to_delivery_period)}";
                    product.dDateFull = $"Срок поставки: {p.from_delivery_period}-{p.to_delivery_period} {industriation_crm.Shared.DaysMonthyearsConverter.GetWeekFormat(p.to_delivery_period)}";
                }
                if (p.delivery_period_type_id == 3)
                {
                    product.dDate = $"Срок поставки: {p.from_delivery_period}-{p.to_delivery_period} {industriation_crm.Shared.DaysMonthyearsConverter.GetMonthFormat(p.to_delivery_period)}";
                    product.dDateFull = $"Срок поставки: {p.from_delivery_period}-{p.to_delivery_period} {industriation_crm.Shared.DaysMonthyearsConverter.GetMonthFormat(p.to_delivery_period)}";
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
                sub_total.value_total = (order?._current_check?._total_product_price.Value / 1.2)!.Value.ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());
                sub_total.value = (order?._current_check?._total_product_price.Value / 1.2)!.Value.ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());
            }
            sub_total.sort_order = "1";
            order_Print_From.order_data.totals_info.Add(sub_total);

            total_info shipping = new total_info();
            shipping.order_id = order.id.ToString();
            shipping.code = "shipping";
            shipping.title = "Самовывоз";
            if (order.delivery?._price != null)
            {
                shipping.value_total =(order.delivery._price.Value / 1.2).ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());
                shipping.value = (order.delivery._price.Value / 1.2).ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());
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
                tax.value = (products_with_delivery_price.Value - products_with_delivery_price.Value / 1.2).ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());
                tax.value_total =  (products_with_delivery_price.Value - products_with_delivery_price.Value / 1.2).ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());
            }
            tax.sort_order = "3";
            order_Print_From.order_data.totals_info.Add(tax);

            total_info total = new total_info();
            total.order_id = order.id.ToString();
            total.code = "total";
            total.title = "Итого с НДС";
            if (products_with_delivery_price != null)
            {
                total.value = products_with_delivery_price.Value.ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());
                total.value_total =  products_with_delivery_price.Value.ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());
                order_Print_From.summ = products_with_delivery_price.Value.ToString("N", industriation_crm.NumberMask.NumberMask.GetNi());
            }
            total.sort_order = "9";
            order_Print_From.order_data.totals_info.Add(total);

            return order_Print_From;
        }
    }
}
