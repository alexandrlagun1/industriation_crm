using industriation_crm.Server.Interfaces;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace industriation_crm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContact _IContact;
        public ContactController(IContact IContact)
        {
            _IContact = IContact;
        }
        [HttpGet]
        public async Task<List<contact>> Get()
        {
            return await Task.FromResult(_IContact.GetContactDetails());
        }
        [HttpGet("GetNoClientContacts")]
        public async Task<List<contact>> GetNoClientContacts(int id)
        {
            return await Task.FromResult(_IContact.GetNoClientContacts());
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            contact contact = _IContact.GetContactData(id);
            if (contact != null)
            {
                return Ok(contact);
            }
            return NotFound();
        }
        [HttpPost]
        public contact Post(contact contact)
        {
            return _IContact.AddContact(contact);
        }
        [HttpPut]
        public void Put(contact contact)
        {
            _IContact.UpdateContactDetails(contact);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _IContact.DeleteContact(id);
            return Ok();
        }
        [HttpPut("update_contact_to_client")]
        public void add_contact_to_client(List<contact> contacts)
        {
            _IContact.UpdateContacts(contacts);
        }

    }
}
