using Demo_1_Ecommerce.Data;
using Demo_1_Ecommerce.Models;
using Demo_1_Ecommerce.Reposatories;
using System.Linq;

namespace Demo_1_Ecommerce.Implementation
{
    public class ContactRepo : GenaricRepo<Contact>, IContactRepo
    {
        private readonly ApplicationDbContext _context;

        public ContactRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Contact contact)
        {
            var contactInDB = _context.Contacts.FirstOrDefault(x => x.Id == contact.Id);
            if (contactInDB != null)
            {
                contactInDB.Name = contact.Name;
                contactInDB.Email = contact.Email;
                contactInDB.Subject = contact.Subject;
                contactInDB.Phone = contact.Phone;
                contactInDB.Message = contact.Message;
                contactInDB.CreatedAt = DateTime.Now; // Or use the original creation date if you want
            }
        }
    }
}
