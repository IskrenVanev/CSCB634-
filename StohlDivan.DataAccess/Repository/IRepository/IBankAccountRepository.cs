using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StohlDivan.Models;

namespace StohlDivan.DataAccess.Repository.IRepository
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        void Update(BankAccount bankAccount);
    }
}
