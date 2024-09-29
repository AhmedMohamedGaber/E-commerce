using Demo_1_Ecommerce.Models;
using Demo_1_Ecommerce.Reposatories;
using Demo_1_Ecommerce.ViewModels;

namespace Demo_1_Ecommerce.Reposatories
{
    public interface IContactRepo : IGenaricRepo<Contact>
    {
        void Update(Contact contact);
    }
}