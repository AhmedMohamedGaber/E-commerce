using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Demo_1_Ecommerce.Reposatories;
using Demo_1_Ecommerce;
using Demo_1_Ecommerce.ViewModels;

namespace Demo_1_Ecommerce.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //[Authorize(Roles = SD.AdminRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/CategoryApi
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _unitOfWork.Category.GetAll();
            return Ok(categories);
        }

        // GET: api/CategoryApi/{id}
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category.GetByID(x => x.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/CategoryApi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Category.add(category);
            _unitOfWork.complete();
            return CreatedAtAction(nameof(GetCategory), new { id = category.id }, category);
        }

        // PUT: api/CategoryApi/{id}
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Category.update(category);
            _unitOfWork.complete();
            return NoContent();
        }

        // DELETE: api/CategoryApi/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var categoryDB = _unitOfWork.Category.GetByID(x => x.id == id);
            if (categoryDB == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.remove(categoryDB);
            _unitOfWork.complete();
            return Ok(new { success = true, message = "Deleted successfully" });
        }
    }
}
