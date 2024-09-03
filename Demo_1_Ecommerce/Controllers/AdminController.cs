//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity;
//using Demo_1_Ecommerce.ViewModels;
//using Demo_1_Ecommerce.Models;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//namespace Demo_1_Ecommerce.Controllers
//{
//    public class AdminController : Controller
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly RoleManager<Role> _roleManager;

//        public AdminController(UserManager<User> userManager, RoleManager<Role> roleManager)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }

//        // GET: Admin/Users
//        public async Task<IActionResult> Users()
//        {
//            var users = _userManager.Users.ToList();
//            var roleIds = users.Select(user => user.RoleId.ToString()).Distinct();
//            var roles = await Task.WhenAll(roleIds.Select(id => _roleManager.FindByIdAsync(id)));

//            var roleDict = roles.Where(role => role != null)
//                                .ToDictionary(role => role.Id, role => role.RoleName);

//            var model = users.Select(user => new LoginViewModel
//            {
//                UserId = user.UserId,
//                Name = user.Name,
//                Username = user.UserName,
//                Mobile = user.Mobile,
//                Email = user.Email,
//                Address = user.Address,
//                PostCode = user.PostCode,
//                RoleName = roleDict.ContainsKey(user.RoleId.ToString()) ? roleDict[user.RoleId.ToString()] : "Unknown"
//            });

//            return View(model);
//        }
//    }
//}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//[Authorize(Policy = "AdminOnly")]
public class AdminController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult Order()
    {
        return View();
    }
}




