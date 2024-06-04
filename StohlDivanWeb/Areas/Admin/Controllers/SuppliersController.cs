using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StohlDivan.DataAccess.Repository.IRepository;
using StohlDivan.Models;
using StohlDivan.Utility;

namespace StohlDivanWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SuppliersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SuppliersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Supplier> objSupplierList = _unitOfWork.Supplier.GetAll().ToList();

            return View("Index", objSupplierList);
        }
        public IActionResult Upsert(int? id)
        {


            if (id == null || id == 0)
            {
                //create
                return View(new Supplier());
            }
            else
            {
                //update
                Supplier SupplierObj = _unitOfWork.Supplier.Get(u => u.Id == id);
                return View(SupplierObj);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Supplier SupplierObj)
        {


            if (ModelState.IsValid)
            {

                if (SupplierObj.Id == 0)
                {
                    _unitOfWork.Supplier.Add(SupplierObj);
                }
                else
                {
                    _unitOfWork.Supplier.Update(SupplierObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Supplier created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                return View("Upsert", SupplierObj);
            }
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Supplier> objSupplierList = _unitOfWork.Supplier.GetAll().ToList();
            return Json(new { data = objSupplierList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var SupplierToBeDeleted = _unitOfWork.Supplier.Get(u => u.Id == id);
            if (SupplierToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Supplier.Remove(SupplierToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
