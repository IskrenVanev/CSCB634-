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
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;
        public ChangeNameController(IUnitOfWork unitOfWork, IConfiguration configuration, IHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _env = env;
        }
        public IActionResult Index()
        {
            var siteName = _configuration["WebsiteSettings:SiteName"];
            ViewBag.SiteName = siteName;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string siteName)
        {
            var filePath = Path.Combine(_env.ContentRootPath, "appsettings.json");

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj["WebsiteSettings"]["SiteName"] = siteName;

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            await System.IO.File.WriteAllTextAsync(filePath, output);

            ViewBag.SiteName = siteName;
            TempData["success"] = "Website name changed successfully";
            return View();
        }
    }
}
