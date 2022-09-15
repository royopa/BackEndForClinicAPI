using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndForClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly BackEndForClinicAPIDBContext dbContext;

        public ContactsController(BackEndForClinicAPIDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            {
                var contact = new Contact()
                {
                    Id = Guid.NewGuid(),
                    Address = addContactRequest.Address,
                    FullName = addContactRequest.FullName,
                    Email = addContactRequest.Email,
                    Phone = addContactRequest.Phone
                };

                await dbContext.Contacts.AddAsync(contact);
                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null)
            {
                dbContext.Contacts.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }

            return NotFound();
        }



        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null)
            {
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;
                contact.Address = updateContactRequest.Address;
                contact.FullName = updateContactRequest.FullName;
                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }

            return NotFound();
        }
    }
}
