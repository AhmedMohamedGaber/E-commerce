using Demo_1_Ecommerce.Data;
using Demo_1_Ecommerce.Models;
using Demo_1_Ecommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Demo_1_Ecommerce.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }


        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Include(p => p.ProductImages)
                .ToListAsync();

            var productViewModels = products.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ShortDescription = p.ShortDescription,
                LongDescription = p.LongDescription,
                Price = p.Price,
                Quantity = p.Quantity,
                CategoryName = p.Category.CategoryName,
                SubCategoryName = p.SubCategory.SubCategoryName,
                ProductImages = p.ProductImages.Select(pi => new ProductImageViewModel
                {
                    ImageId = pi.ImageId,
                    ImageUrl = pi.ImageUrl
                }).ToList()
            }).ToList();

            return View(productViewModels);
        }



        // GET: Products/Details/5
        // In ProductsController.cs
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryName = product.Category.CategoryName,
                SubCategoryName = product.SubCategory.SubCategoryName,
                ProductImages = product.ProductImages.Select(pi => new ProductImageViewModel
                {
                    ImageId = pi.ImageId,
                    ImageUrl = pi.ImageUrl
                }).ToList()
            };

            return View(model);
        }


        // GET: Products/Create
        public IActionResult Create()
        {
            var model = new ProductViewModel
            {
                ProductImages = new List<ProductImageViewModel>() // Initialize if needed
            };

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewBag.SubCategories = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName");

            return View(model);
        }


        // POST: Products/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductName = model.ProductName,
                    ShortDescription = model.ShortDescription,
                    LongDescription = model.LongDescription,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    CategoryId = model.CategoryId,
                    SubCategoryId = model.SubCategoryId,
                    CreatedDate = DateTime.Now, // Set created date
                    IsActive = true // Set default value
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                if (model.ImageFiles != null && model.ImageFiles.Any())
                {
                    foreach (var file in model.ImageFiles)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var productImage = new ProductImage
                            {
                                ProductId = product.ProductId,
                                ImageUrl = $"/images/{fileName}",
                                DefaultImage = false // Set default value if needed
                            };

                            _context.ProductImages.Add(productImage);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            // Repopulate dropdowns if validation fails
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewBag.SubCategories = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName");

            return View(model);
        }






        // Add Edit, Delete, etc. actions

        // In ProductsController.cs
        // In ProductsController.cs
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                // Add other properties as needed
            };

            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", model.CategoryId);
            ViewData["SubCategories"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", model.SubCategoryId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                product.ProductName = model.ProductName;
                product.ShortDescription = model.ShortDescription;
                product.LongDescription = model.LongDescription;
                product.Price = model.Price;
                product.Quantity = model.Quantity;
                product.CategoryId = model.CategoryId;
                product.SubCategoryId = model.SubCategoryId;
                // Update other properties as needed

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", model.CategoryId);
            ViewData["SubCategories"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", model.SubCategoryId);
            return View(model);
        }


        



        // In ProductsController.cs
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                // Optionally delete associated images if needed
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }

}
