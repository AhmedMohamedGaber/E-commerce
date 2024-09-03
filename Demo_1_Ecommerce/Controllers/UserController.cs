//using Microsoft.AspNetCore.Mvc;
//using Demo_1_Ecommerce.Data;
//using Demo_1_Ecommerce.Models;
//using Demo_1_Ecommerce.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//[Authorize(Roles = "Admin")]
//public class UserController : Controller
//{
//    private readonly ApplicationDbContext _context;
//    private readonly UserManager<User> _userManager;
//    private readonly RoleManager<Role> _roleManager;

//    public UserController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
//    {
//        _context = context;
//        _userManager = userManager;
//        _roleManager = roleManager;
//    }

//    // GET: User
//    public IActionResult Index()
//    {
//        var users = _context.Users.Include(u => u.Role).ToList();
//        var userViewModels = users.Select(u => new UserViewModel
//        {
//            UserId = u.Id,
//            Name = u.Name,
//            Username = u.UserName,
//            Mobile = u.Mobile,
//            Email = u.Email,
//            Address = u.Address,
//            PostCode = u.PostCode,
//            CreatedDate = u.CreatedDate,
//            RoleId = u.RoleId,
//            RoleName = u.Role?.RoleName,
//            Roles = _roleManager.Roles.Select(r => new SelectListItem
//            {
//                Value = r.Id,
//                Text = r.Name
//            }).ToList()
//        }).ToList();
//        return View(userViewModels);
//    }

//    // GET: User/Create
//    public IActionResult Create()
//    {
//        var model = new UserViewModel
//        {
//            Roles = _roleManager.Roles.Select(r => new SelectListItem
//            {
//                Value = r.Id,
//                Text = r.Name
//            }).ToList()
//        };
//        return View(model);
//    }

//    // POST: User/Create
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Create(UserViewModel model)
//    {
//        if (ModelState.IsValid)
//        {
//            var user = new User
//            {
//                UserName = model.Username,
//                Email = model.Email,
//                Name = model.Name,
//                Mobile = model.Mobile,
//                Address = model.Address,
//                PostCode = model.PostCode,
//                ImageUrl = model.ImageUrl,
//                RoleId = model.RoleId,
//                CreatedDate = DateTime.Now
//            };
//            var result = await _userManager.CreateAsync(user, model.Password);
//            if (result.Succeeded)
//            {
//                var role = await _roleManager.FindByIdAsync(model.RoleId.ToString());
//                if (role != null)
//                {
//                    await _userManager.AddToRoleAsync(user, role.Name);
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            }
//        }
//        model.Roles = _roleManager.Roles.Select(r => new SelectListItem
//        {
//            Value = r.Id,
//            Text = r.Name
//        }).ToList();
//        return View(model);
//    }

//    // GET: User/Edit/5
//    public async Task<IActionResult> Edit(string id)
//    {
//        var user = await _userManager.FindByIdAsync(id);
//        if (user == null) return NotFound();

//        var model = new UserViewModel
//        {
//            UserId = user.Id,
//            Name = user.Name,
//            Username = user.UserName,
//            Mobile = user.Mobile,
//            Email = user.Email,
//            Address = user.Address,
//            PostCode = user.PostCode,
//            CreatedDate = user.CreatedDate,
//            RoleId = user.RoleId,
//            RoleName = (await _roleManager.FindByIdAsync(user.RoleId.ToString()))?.Name,
//            Roles = _roleManager.Roles.Select(r => new SelectListItem
//            {
//                Value = r.Id,
//                Text = r.Name
//            }).ToList()
//        };
//        return View(model);
//    }

//    // POST: User/Edit/5
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Edit(UserViewModel model)
//    {
//        if (ModelState.IsValid)
//        {
//            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
//            if (user == null) return NotFound();

//            user.Name = model.Name;
//            user.UserName = model.Username;
//            user.Mobile = model.Mobile;
//            user.Email = model.Email;
//            user.Address = model.Address;
//            user.PostCode = model.PostCode;
//            user.RoleId = model.RoleId;

//            var result = await _userManager.UpdateAsync(user);
//            if (result.Succeeded)
//            {
//                // Remove user from previous role
//                var oldRole = await _roleManager.FindByIdAsync(user.RoleId.ToString());
//                if (oldRole != null)
//                {
//                    await _userManager.RemoveFromRoleAsync(user, oldRole.Name);
//                }

//                // Add user to new role
//                var newRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
//                if (newRole != null)
//                {
//                    await _userManager.AddToRoleAsync(user, newRole.Name);
//                }

//                return RedirectToAction(nameof(Index));
//            }
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            }
//        }
//        model.Roles = _roleManager.Roles.Select(r => new SelectListItem
//        {
//            Value = r.Id,
//            Text = r.Name
//        }).ToList();
//        return View(model);
//    }

//    // GET: User/Delete/5
//    public async Task<IActionResult> Delete(string id)
//    {
//        var user = await _userManager.FindByIdAsync(id);
//        if (user == null) return NotFound();

//        var model = new UserViewModel
//        {
//            UserId = user.Id,
//            Name = user.Name,
//            Username = user.UserName,
//            Mobile = user.Mobile,
//            Email = user.Email,
//            Address = user.Address,
//            PostCode = user.PostCode,
//            CreatedDate = user.CreatedDate,
//            RoleId = user.RoleId,
//            RoleName = (await _roleManager.FindByIdAsync(user.RoleId.ToString()))?.Name
//        };
//        return View(model);
//    }

//    // POST: User/Delete/5
//    [HttpPost, ActionName("Delete")]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> DeleteConfirmed(string id)
//    {
//        var user = await _userManager.FindByIdAsync(id);
//        if (user != null)
//        {
//            var result = await _userManager.DeleteAsync(user);
//            if (result.Succeeded)
//            {
//                return RedirectToAction(nameof(Index));
//            }
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            }
//        }
//        return RedirectToAction(nameof(Index));
//    }
//}
