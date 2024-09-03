using Microsoft.AspNetCore.Mvc;
using Demo_1_Ecommerce.Data;
using Demo_1_Ecommerce.Models;
using Demo_1_Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly ApplicationDbContext _context;

    public RoleController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Role
    public IActionResult Index()
    {
        var roles = _context.Roles.ToList();
        var roleViewModels = roles.Select(r => new RoleViewModel
        {
            RoleId = r.RoleId,
            RoleName = r.RoleName,
            Users = r.Users.Select(u => new UserViewModel
            {
                UserId = int.Parse(u.Id),
                Name = u.Name,
                Username = u.Username,
                Email = u.Email,
                Mobile = u.Mobile
                // Add other properties as needed
            }).ToList()
        }).ToList();
        return View(roleViewModels);
    }

    // GET: Role/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Role/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(RoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var role = new Role
            {
                RoleName = model.RoleName
            };
            _context.Roles.Add(role);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // GET: Role/Edit/5
    public IActionResult Edit(int id)
    {
        var role = _context.Roles.Find(id);
        if (role == null) return NotFound();

        var model = new RoleViewModel
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName
            // Include other properties as needed
        };
        return View(model);
    }

    // POST: Role/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(RoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var role = _context.Roles.Find(model.RoleId);
            if (role == null) return NotFound();

            role.RoleName = model.RoleName;
            _context.Roles.Update(role);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // GET: Role/Delete/5
    public IActionResult Delete(int id)
    {
        var role = _context.Roles.Find(id);
        if (role == null) return NotFound();

        var model = new RoleViewModel
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName
        };
        return View(model);
    }

    // POST: Role/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var role = _context.Roles.Find(id);
        if (role != null)
        {
            _context.Roles.Remove(role);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}
