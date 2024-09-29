using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Demo_1_Ecommerce.Models;
using Demo_1_Ecommerce.Reposatories;

namespace Demo_1_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var contactList = _unitOfWork.Contact.GetAll();
            return View(contactList); // Ensure this returns IEnumerable<Contact>
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Returns the create view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreatedAt = DateTime.Now;
                _unitOfWork.Contact.add(contact);
                _unitOfWork.complete();

                TempData["Message"] = "Your message has been sent successfully!";
                TempData["Type"] = "success"; // Optional for displaying message type
                return RedirectToAction("Index"); // Redirect to the Index page after creation
            }

            return View(contact); // Return the same view with the model if validation fails
        }



        // Details action
        [HttpGet]
        public IActionResult Details(int id)
        {
            var contact = _unitOfWork.Contact.GetByID(c => c.Id == id); // Adjust property as needed
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact); // Pass the contact to the Details view
        }




        // Get the Delete confirmation view
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var contact = _unitOfWork.Contact.GetByID(c => c.Id == id); // Adjust property as needed
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact); // Pass the contact to the Delete confirmation view
        }

        // Delete action
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var contact = _unitOfWork.Contact.GetByID(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            _unitOfWork.Contact.remove(contact);
            _unitOfWork.complete();
            TempData["Type"] = "error";
            TempData["message"] = "Deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
