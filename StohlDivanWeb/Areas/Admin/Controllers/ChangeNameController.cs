using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StohlDivan.DataAccess.Repository.IRepository;
using StohlDivan.Utility;

namespace StohlDivanWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ChangeNameController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public ChangeNameController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
