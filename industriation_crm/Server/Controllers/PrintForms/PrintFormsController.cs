using industriation_crm.Client.PrintForms;
using industriation_crm.Client.PrintForms.data;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static System.Net.WebRequestMethods;


namespace industriation_crm.Server.Controllers.PrintForms
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintFormsController : ControllerBase
    {
        [HttpPost]
        public async Task<Stream> GetOrderPrintForm(order_print_form order_print_form)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync($"https://industriation.ru/index.php?route=checkout/ppa_score_pdf&order_id={order_print_form.order_id}&CRM=1&file_method=D", order_print_form);
                var file = await response.Content.ReadAsStreamAsync();
                return file;
            }
        }
        [HttpPost("GetDogovorPrintForm")]
        public async Task<Stream> GetDogovorPrintForm(dogovor_print_form dogovor_print_form)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync($"https://industriation.ru/index.php?route=print_form/print_form/pdf&form=dogovor", dogovor_print_form);
                var file = await response.Content.ReadAsStreamAsync();
                return file;
            }
        }
    }
}
