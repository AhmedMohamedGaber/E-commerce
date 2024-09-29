using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Demo_1_Ecommerce.Models;
using Demo_1_Ecommerce.Reposatories;
using System.Collections.Generic;

namespace Demo_1_Ecommerce.Areas.Admin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize(Roles = SD.AdminRole)]
	public class ContactApiController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public ContactApiController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// GET: api/ContactApi
		[HttpGet]
		public ActionResult<IEnumerable<Contact>> GetAll()
		{
			var contactList = _unitOfWork.Contact.GetAll();
			return Ok(contactList); // Return all contacts as a JSON response
		}

		// GET: api/ContactApi/5
		[HttpGet("{id}")]
		public ActionResult<Contact> GetById(int id)
		{
			var contact = _unitOfWork.Contact.GetByID(c => c.Id == id);
			if (contact == null)
			{
				return NotFound();
			}
			return Ok(contact); // Return the contact as a JSON response
		}

		// POST: api/ContactApi
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult<Contact> Create([FromBody] Contact contact)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // Return validation errors
			}

			contact.CreatedAt = DateTime.Now;
			_unitOfWork.Contact.add(contact);
			_unitOfWork.complete();

			return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact); // Return the created contact
		}

		// DELETE: api/ContactApi/5
		[HttpDelete("{id}")]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			var contact = _unitOfWork.Contact.GetByID(c => c.Id == id);
			if (contact == null)
			{
				return NotFound();
			}

			_unitOfWork.Contact.remove(contact);
			_unitOfWork.complete();

			return NoContent(); // Return 204 No Content on successful deletion
		}
	}
}
