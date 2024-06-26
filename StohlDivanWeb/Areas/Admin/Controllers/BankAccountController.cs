using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StohlDivan.DataAccess.Repository.IRepository;
using StohlDivan.Models;
using StohlDivan.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StohlDivanWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BankAccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<BankAccount> bankAccounts = _unitOfWork.BankAccount.GetAll(includeProperties: "User").ToList();
            return View("Index", bankAccounts);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Expression<Func<BankAccount, bool>> filter = ba => ba.Id == id.Value;
            BankAccount bankAccount = _unitOfWork.BankAccount.Get(filter, includeProperties: "User");

            if (bankAccount == null)
            {
                return NotFound();
            }

            return View("Delete", bankAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Expression<Func<BankAccount, bool>> filter = ba => ba.Id == id.Value;
            BankAccount bankAccount = _unitOfWork.BankAccount.Get(filter);

            if (bankAccount == null)
            {
                return NotFound();
            }

            _unitOfWork.BankAccount.Remove(bankAccount);
            _unitOfWork.Save();
            TempData["success"] = "Bank Account deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
