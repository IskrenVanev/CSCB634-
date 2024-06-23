using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace StohlDivan.Models.ViewModels
    {
        public class StatisticsVM
        {
            public int TotalProducts { get; set; }
            public double TotalSales { get; set; }
            public int TotalOrders { get; set; }
            public double AverageOrderValue { get; set; }
            public Dictionary<string, double> SalesByCategory { get; set; }
            public List<Supplier> Suppliers { get; set; }
            public List<IdentityUser> Users { get; set; }
            public List<ProductSalesData> TopSellingProducts { get; set; }
            public List<SalesTrendData> SalesTrends { get; set; }
            public List<CustomerSalesData> TopCustomers { get; set; }
        }

        public class ProductSalesData
        {
            public string ProductName { get; set; }
            public double Sales { get; set; }
        }

        public class SalesTrendData
        {
            public DateTime Date { get; set; }
            public double Sales { get; set; }
        }

        public class CustomerSalesData
        {
            public string CustomerName { get; set; }
            public double Sales { get; set; }
        }
    }