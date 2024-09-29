using Demo_1_Ecommerce;
using Demo_1_Ecommerce.Models;
using Demo_1_Ecommerce.Reposatories;
using Demo_1_Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Demo_1_Ecommerce.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //[Authorize(Roles = SD.AdminRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductApiController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        

        // GET: api/ProductApi
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _unitOfWork.Product.GetAll().Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                ImgUrl = $"{Request.Scheme}://{Request.Host}/{p.img}", // Full URL
                p.Price,
                p.CategoryId,
                Category = p.Category // Include category if loaded
            });

            return Ok(products);
        }

        // GET: api/ProductApi/{id}
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _unitOfWork.Product.GetByID(x => x.Id == id, icludeWord: "Category");
            if (product == null)
            {
                return NotFound();
            }

            var result = new
            {
                product.Id,
                product.Name,
                product.Description,
                ImgUrl = $"{Request.Scheme}://{Request.Host}/images/{product.img}", // Full URL
                product.Price,
                product.CategoryId,
                Category = product.Category // Include category details
            };

            return Ok(result);
        }


        // POST: api/ProductApi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct([FromForm] Product productFReq, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string rootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(rootPath, "images"); // Specify your image directory
                var ext = Path.GetExtension(file.FileName);
                using (var filestream = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }
                productFReq.img = $"{fileName}{ext}"; // Store only the filename
            }

            _unitOfWork.Product.add(productFReq);
            _unitOfWork.complete();

            return CreatedAtAction(nameof(GetProduct), new { id = productFReq.Id }, productFReq);
        }

        // PUT: api/ProductApi/{id}
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProduct(int id, [FromForm] Product product, IFormFile? file)
        {
            if (id != product.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string rootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(rootPath, "images"); // Specify your image directory
                var ext = Path.GetExtension(file.FileName);

                if (product.img != null)
                {
                    var oldImgPath = Path.Combine(rootPath, product.img.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }

                using (var filestream = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }
                product.img = $"{fileName}{ext}"; // Store only the filename
            }

            _unitOfWork.Product.update(product);
            _unitOfWork.complete();
            return NoContent();
        }

        // DELETE: api/ProductApi/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var productDB = _unitOfWork.Product.GetByID(x => x.Id == id);
            if (productDB == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.remove(productDB);
            var oldImgPath = Path.Combine(_webHostEnvironment.WebRootPath, productDB.img.TrimStart('\\'));
            DeleteFile(oldImgPath); // Use the private method for file deletion

            _unitOfWork.complete();
            return Ok(new { success = true, message = "Product deleted successfully" });
        }

        private void DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
