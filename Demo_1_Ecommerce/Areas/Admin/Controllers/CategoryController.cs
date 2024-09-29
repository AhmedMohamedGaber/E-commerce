using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Demo_1_Ecommerce.Reposatories;
using Demo_1_Ecommerce;
using Demo_1_Ecommerce.ViewModels;

namespace Demo_1_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]

    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var CategoryList = _unitOfWork.Category.GetAll();

            return View(CategoryList);
        }

        [HttpGet]
        public IActionResult Create()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {

                _unitOfWork.Category.add(category);
                _unitOfWork.complete();
                TempData["Type"] = "success";
                TempData["message"] = "created successfully";
                return RedirectToAction("Index");

            }


            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                Category CategoryFromDataBase = _unitOfWork.Category.GetByID(x => x.id == id);
                // categorys.Update(id, CategoryFromDataBase);
                // categorys.Save();
                return View(CategoryFromDataBase);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {

            int IDFromDataBase = category.id;


            _unitOfWork.Category.update(category);
            _unitOfWork.complete();
            TempData["Type"] = "info";
            TempData["message"] = "Updated successfully";
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            Category CategoryFromDataBase = _unitOfWork.Category.GetByID(x => x.id == id);
            return View(CategoryFromDataBase);


        }
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var categoryDB = _unitOfWork.Category.GetByID(x => x.id == id);
            if (categoryDB == null)
            {
                NotFound();
            }
            _unitOfWork.Category.remove(categoryDB);
            _unitOfWork.complete();
            TempData["Type"] = "error";
            TempData["message"] = "Deleted successfully";
            return RedirectToAction("index");
        }

    }
}