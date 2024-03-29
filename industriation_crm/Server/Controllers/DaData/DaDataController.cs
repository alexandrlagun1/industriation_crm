﻿using industriation_crm.Shared.DaData;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace industriation_crm.Server.Controllers.DaData
{
    [Route("api/[controller]")]
    [ApiController]
    public class DaDataController : ControllerBase
    {
        [HttpGet("{inn}")]
        public async Task<client?> Get(long inn)
        {
            client client = new client();
            DaDataContent? daDataContent = new();
            using (var httpClient = new HttpClient())
            {
                DaDataRequest daDataRequest = new DaDataRequest();
                daDataRequest.query = inn.ToString();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Token 101a2b762b4a480848f5aaa31be699282839126a");
                httpClient.DefaultRequestHeaders.Add("X-Secret", "ee184ecb34e7b179750e6df20129463f5b60eecd");

                var response = await httpClient.PostAsJsonAsync("https://suggestions.dadata.ru/suggestions/api/4_1/rs/findById/party", daDataRequest);
                daDataContent = await response.Content.ReadFromJsonAsync<DaDataContent>();
                if (daDataContent?.suggestions?.Count == 0)
                    return client;
            }
            
            client.org_ogrn = Convert.ToInt64(daDataContent?.suggestions?[0]?.data?.ogrn);
            if (String.IsNullOrEmpty(daDataContent?.suggestions?[0]?.data?.name?.full_with_opf))
                client.org_name = daDataContent?.suggestions?[0]?.value;
            else
                client.org_name = daDataContent?.suggestions?[0]?.data?.name?.full_with_opf;
            client.org_address = daDataContent?.suggestions?[0]?.data?.address?.value;
            client.org_kpp = Convert.ToInt64(daDataContent?.suggestions?[0]?.data?.kpp);
            return client;
        }
    }
    public class DaDataRequest
    {
        public string? query { get; set; }
    }
}
