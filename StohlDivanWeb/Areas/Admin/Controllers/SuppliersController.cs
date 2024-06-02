using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StohlDivan.Utility;

namespace StohlDivanWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SuppliersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
