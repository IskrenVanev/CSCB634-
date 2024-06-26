using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StohlDivan.Models.ViewModels
{
    public class BankAccountVM
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Bank Name")]
        public string BankName { get; set; }

        [Required]
        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [Required]
        [DisplayName("IBAN")]
        public string IBAN { get; set; }

        [Required]
        [DisplayName("Swift Code")]
        public string SwiftCode { get; set; }

        public string UserId { get; set; }
    }
}
