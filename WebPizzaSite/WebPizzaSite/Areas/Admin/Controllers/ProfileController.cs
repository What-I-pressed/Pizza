using Microsoft.AspNetCore.Mvc;

namespace WebPizzaSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProfileController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
