using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClient _IClient;
        public ClientController(IClient IClient)
        {
            _IClient = IClient;
        }
        [HttpGet]
        public async Task<List<client>> Get()
        {
            List<client> clients = await Task.FromResult(_IClient.GetClientDetails());
            return clients;

        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            client client = _IClient.GetClientData(id);
            if (client != null)
            {
                return Ok(client);
            }
            return NotFound();
        }
        [HttpPost]
        public int Post(client client)
        {
            return _IClient.AddClient(client);
        }
        [HttpPut]
        public void Put(client client)
        {
            _IClient.UpdateClientDetails(client);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _IClient.DeleteClient(id);
            return Ok();
        }
    }
}
