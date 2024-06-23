using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StohlDivan.DataAccess.Data;
using StohlDivan.Models;
using StohlDivan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StohlDivan.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StatisticsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var statistics = await GetStatisticsAsync();
            return View(statistics);
        }

        private async Task<StatisticsVM> GetStatisticsAsync()
        {
            var totalProducts = await _context.Products.CountAsync();
            var totalOrders = await _context.OrderHeaders.CountAsync();
            var totalSales = await _context.OrderDetails.SumAsync(od => od.Price * od.Count);
            var averageOrderValue = totalSales / totalOrders;

            var salesByCategory = await _context.OrderDetails
                .Include(od => od.Product)
                .ThenInclude(p => p.Category)
                .GroupBy(od => od.Product.Category.Name)
                .Select(g => new { Category = g.Key, Sales = g.Sum(od => od.Price * od.Count) })
                .ToDictionaryAsync(g => g.Category, g => g.Sales);

            var suppliers = await _context.Suppliers.ToListAsync();
            var users = await _userManager.Users.ToListAsync();

            var topSellingProducts = await _context.OrderDetails
                          .Include(od => od.Product)
                          .GroupBy(od => od.Product.Title)
                          .Select(g => new ProductSalesData { ProductName = g.Key, Sales = g.Sum(od => od.Price * od.Count) })
                          .OrderByDescending(ps => ps.Sales)
                          .Take(10)
                          .ToListAsync();

            var salesTrends = await _context.OrderHeaders
                .Include(oh => oh.OrderDetails)
                .SelectMany(oh => oh.OrderDetails, (oh, od) => new { oh.OrderDate, TotalAmount = od.Price * od.Count })
                .GroupBy(x => x.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Sales = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(st => st.Date)
                .Select(st => new SalesTrendData
                {
                    Date = st.Date,
                    Sales = st.Sales
                })
                .ToListAsync();

            var topCustomers = await _context.OrderHeaders
                .Include(oh => oh.ApplicationUser)
                .SelectMany(oh => oh.OrderDetails, (oh, od) => new { oh.ApplicationUser.UserName, TotalAmount = od.Price * od.Count })
                .GroupBy(x => x.UserName)
                .Select(g => new
                {
                    CustomerName = g.Key,
                    Sales = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.Sales)
                .Take(10)
                .ToListAsync();

            // Project the result into CustomerSalesData if needed
            var topCustomerSalesData = topCustomers.Select(x => new CustomerSalesData
            {
                CustomerName = x.CustomerName,
                Sales = x.Sales
            }).ToList();


            return new StatisticsVM
            {
                TotalProducts = totalProducts,
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                AverageOrderValue = averageOrderValue,
                SalesByCategory = salesByCategory,
                Suppliers = suppliers,
                Users = users,
                TopSellingProducts = topSellingProducts,
                SalesTrends = salesTrends,
                TopCustomers = topCustomerSalesData
            };
        }
    }
}
