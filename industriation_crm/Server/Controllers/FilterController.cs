using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Retail;
using industriation_crm.Server.SignalRNotification;
using industriation_crm.Shared.FilterModels;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly IUser _IUser;

        public FilterController(IUser IUser)
        {
            _IUser = IUser;
        }
        [HttpPut("SaveOrdersFilter")]
        public void SaveOrdersFilter(OrdersFilter ordersFilter)
        {
            _IUser.SaveOrdersFilter(ordersFilter);
        }
        [HttpGet("GetOrdersFilter/{user_id}")]
        public OrdersFilter GetOrdersFilter(int user_id)
        {
            return _IUser.GetOrdersFilter(user_id);
        }
        [HttpPut("SaveClientsFilter")]
        public void SaveClientsFilter(ClientFilter clientFilter)
        {
            _IUser.SaveClientsFilter(clientFilter);
        }
        [HttpGet("GetClientsFilter/{user_id}")]
        public ClientFilter GetClientsFilter(int user_id)
        {
            return _IUser.GetClientsFilter(user_id);
        }
        [HttpPut("SaveProductsFilter")]
        public void SaveProductsFilter(ProductFilter productFilter)
        {
            _IUser.SaveProductsFilter(productFilter);
        }
        [HttpGet("GetProductsFilter/{user_id}")]
        public ProductFilter GetProductsFilter(int user_id)
        {
            return _IUser.GetProductsFilter(user_id);
        }
        [HttpPut("SaveCallHistoryFilter")]
        public void SaveCallHistoryFilter(CallHistoryFilter callHistoryFilter)
        {
            _IUser.SaveCallHistoryFilter(callHistoryFilter);
        }
        [HttpGet("GetCallHistoryFilter/{user_id}")]
        public CallHistoryFilter GetCallHistoryFilter(int user_id)
        {
            return _IUser.GetCallHistoryFilter(user_id);
        }
    }
}
