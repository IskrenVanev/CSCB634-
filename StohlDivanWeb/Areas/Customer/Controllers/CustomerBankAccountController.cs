using StohlDivan.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StohlDivan.DataAccess.Repository.IRepository;
using StohlDivan.Models;
using StohlDivan.Models.ViewModels;
using StohlDivan.Utility;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace StohlDivanWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Customer)]
    public class CustomerBankAccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CustomerBankAccountController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var bankAccounts = _unitOfWork.BankAccount.GetAll(b => b.UserId == user.Id)
                .Select(b => new BankAccountVM
                {
                    Id = b.Id,
                    BankName = b.BankName,
                    AccountNumber = b.AccountNumber,
                    IBAN = b.IBAN,
                    SwiftCode = b.SwiftCode,
                    UserId = b.UserId
                }).ToList();

            return View(bankAccounts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BankAccountVM bankAccountVM)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var bankAccount = new BankAccount
                {
                    BankName = bankAccountVM.BankName,
                    AccountNumber = bankAccountVM.AccountNumber,
                    IBAN = bankAccountVM.IBAN,
                    SwiftCode = bankAccountVM.SwiftCode,
                    UserId = user.Id
                };

                _unitOfWork.BankAccount.Add(bankAccount);
                _unitOfWork.Save();
                TempData["success"] = "Bank account created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccountVM);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var bankAccount = _unitOfWork.BankAccount.Get(u => u.Id == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            var bankAccountVM = new BankAccountVM
            {
                Id = bankAccount.Id,
                BankName = bankAccount.BankName,
                AccountNumber = bankAccount.AccountNumber,
                IBAN = bankAccount.IBAN,
                SwiftCode = bankAccount.SwiftCode,
                UserId = bankAccount.UserId
            };

            return View(bankAccountVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BankAccountVM bankAccountVM)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var bankAccount = _unitOfWork.BankAccount.Get(u => u.Id == bankAccountVM.Id);
                if (bankAccount == null)
                {
                    return NotFound();
                }

                // Ensure the bank account belongs to the current user
                if (bankAccount.UserId != user.Id)
                {
                    return Unauthorized();
                }

                bankAccount.BankName = bankAccountVM.BankName;
                bankAccount.AccountNumber = bankAccountVM.AccountNumber;
                bankAccount.IBAN = bankAccountVM.IBAN;
                bankAccount.SwiftCode = bankAccountVM.SwiftCode;

                _unitOfWork.BankAccount.Update(bankAccount);
                _unitOfWork.Save();

                TempData["success"] = "Bank account updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccountVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var bankAccount = _unitOfWork.BankAccount.Get(u => u.Id == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            var bankAccountVM = new BankAccountVM
            {
                Id = bankAccount.Id,
                BankName = bankAccount.BankName,
                AccountNumber = bankAccount.AccountNumber,
                IBAN = bankAccount.IBAN,
                SwiftCode = bankAccount.SwiftCode,
                UserId = bankAccount.UserId
            };

            return View(bankAccountVM);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var bankAccount = _unitOfWork.BankAccount.Get(u => u.Id == id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            // Ensure the bank account belongs to the current user
            if (bankAccount.UserId != user.Id)
            {
                return Unauthorized();
            }

            _unitOfWork.BankAccount.Remove(bankAccount);
            _unitOfWork.Save();
            TempData["success"] = "Bank account deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
