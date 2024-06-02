using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StohlDivan.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        public string SupplierName { get; set; }

        public OrderHeader OrderHeader { get; set; }
    }
}
