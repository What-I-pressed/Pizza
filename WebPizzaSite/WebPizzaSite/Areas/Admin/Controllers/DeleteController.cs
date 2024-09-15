using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebPizzaSite.Areas.Admin.Models;
using WebPizzaSite.Constants;
using WebPizzaSite.Data.Entities.Identity;

namespace WebPizzaSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class DeleteController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;

        public DeleteController(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser()
        {
            var users = _userManager.Users.Select(user => new AdminPanelViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName
            }).ToList();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("DeleteUser");
                }
                else
                {
                    // Handle error during deletion (e.g., log or show a message)
                    ModelState.AddModelError("", "Failed to delete user.");
                }
            }

            return RedirectToAction("DeleteUser");
        }
    }
}
