using industriation_crm.Client.PrintForms.data;
using industriation_crm.Shared.Models;
using industriation_crm.Shared.PrintForms.data;

namespace industriation_crm.Client.PrintForms.converters
{
    public static class dogovor_converter
    {
        public static dogovor_print_form ConvertDogovorPrintForm(order? order, string pch)
        {
            dogovor_print_form dogovor_print_form = new dogovor_print_form();
            dogovor_print_form.add_date = $"{order?.order_date?.ToString("dd")}.{order?.order_date?.ToString("MM")}.{order?.order_date?.ToString("yyyy")}";
            dogovor_print_form.pch = pch;
            dogovor_print_form.order_id = $"{order.id.ToString()}";

            dogovor_print_form.contragent = new contragent();
            dogovor_print_form.contragent.user_name = order.client?.contacts?.Where(c => c.main_contact == 1).FirstOrDefault()?.full_name;
            dogovor_print_form.contragent.email = order.client?.contacts?.Where(c => c.main_contact == 1).FirstOrDefault()?.email;
            dogovor_print_form.contragent.phone = order.client?.contacts?.Where(c => c.main_contact == 1).FirstOrDefault()?.phone;
            dogovor_print_form.contragent.kpp = order.client?.org_kpp.ToString();
            dogovor_print_form.contragent.address = order.client?.org_address;
            dogovor_print_form.contragent.rs = order.client?.bank_ras_schet;
            dogovor_print_form.contragent.banck = order.client?.bank_name;
            dogovor_print_form.contragent.bik = order.client?.bank_bik.ToString();
            dogovor_print_form.contragent.inn = order.client?.org_inn.ToString();
            dogovor_print_form.contragent.provider = order.client?.org_name;
            dogovor_print_form.contragent.ks = order.client?.bank_cor_schet;


            return dogovor_print_form;
        }
    }
}
