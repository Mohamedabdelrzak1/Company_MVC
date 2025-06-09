using Company.DAL.Models;
using Company_MVC.Dtos;
using Company_MVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

namespace Company_MVC.Controllers
{
    
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {

            IEnumerable<RoleToReturnDto> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
                users = _roleManager.Roles.Select(U => new RoleToReturnDto
                {
                    Id = U.Id,
                    Name = U.Name,

                });
            }
            else
            {
                users = _roleManager.Roles.Select(U => new RoleToReturnDto
                {
                    Id = U.Id,
                    Name = U.Name,
                }).Where(R => R.Name.ToLower().Contains(SearchInput.ToLower()));

            }
           return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var existingRole = await _roleManager.FindByNameAsync(model.Name);
                if (existingRole == null)
                {
                    var role = new IdentityRole
                    {
                        Name = model.Name
                    };

                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Role created successfully.";
                        TempData["MessageType"] = "create";
                        return RedirectToAction(nameof(Index));
                    }

                    // لو فيه أخطاء في الإنشاء، ضيفها للموديل
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "This role already exists.");
                }
            }

            return View(model);
        }



           [HttpGet]
            public async Task<IActionResult> Details(string? id, string viewName = "Details")
            {
                if (id is null) return BadRequest("Invalid Id");

                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = $"Role with Id: {id} is not found"
                    });

                var dto = new RoleToReturnDto()
                {
                    Id = role.Id,
                    Name = role.Name,
                };

                return View(viewName, dto);
            }


            [HttpGet]
            public async Task<IActionResult> Edit(string? id)
            {
                return await Details(id, viewName: "Edit");
            }     


            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model)
            {
                if (ModelState.IsValid)
                {
                    if (id != model.Id)
                        return BadRequest("Invalid Operation!");

                    var role = await _roleManager.FindByIdAsync(id);
                    if (role == null)
                        return BadRequest("Invalid Operation!");

                    var roleResult = await _roleManager.FindByNameAsync(model.Name);
                    if (roleResult is not null)
                    {
                        role.Name = model.Name;
                        var result = await _roleManager.UpdateAsync(role);
                        if (result.Succeeded)
                        {
                        TempData["Message"] = "Role updated successfully.";
                        TempData["MessageType"] = "edit";
                        return RedirectToAction(nameof(Index));
                        
                        }
                    }

                    ModelState.AddModelError("", "Invalid Operation!");
                }

                return View(model);
            }


            [HttpGet]
            public async Task<IActionResult> Delete(string? id)
            {
                return await Details(id, viewName: "Delete");
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id, RoleToReturnDto model)
            {
                if (string.IsNullOrEmpty(id) || id != model.Id)
                    return BadRequest("Invalid Role ID");

                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return NotFound("Role not found");

                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Role deleted successfully.";
                    TempData["MessageType"] = "delete";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                // إذا حصلت أخطاء، نرجع للمستخدم نفس View الحذف
                return View("Delete", model);
            }

           [HttpGet]
           public async Task<IActionResult> AddOrRemoveUsers(string roleId)
           {
            if (string.IsNullOrWhiteSpace(roleId))
                return BadRequest("Role Id is required.");

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();

            var usersInRole = new List<UsersInRoleViewDto>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewDto()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };

                usersInRole.Add(userInRole);
            }

            
            ViewBag.RoleId = roleId;

            return View(usersInRole);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrRemoveUsers(List<UsersInRoleViewDto> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            foreach (var userModel in model)
            {
                var user = await _userManager.FindByIdAsync(userModel.UserId);
                if (user == null) continue;

                if (userModel.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userModel.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            TempData["Message"] = "Role updated successfully";
            TempData["MessageType"] = "edit"; 
            return RedirectToAction("Index"); // أو لأي مكان تاني
        }


    }
}

