using Microsoft.EntityFrameworkCore;
using StohlDivan.DataAccess.Data;
using StohlDivan.DataAccess.Repository.IRepository;
using StohlDivan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StohlDivan.DataAccess.Repository
{
    public class BankAccountRepository : Repository<BankAccount>, IBankAccountRepository
    {
        private ApplicationDbContext _context;

        public BankAccountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(BankAccount bankAccount)
        {
            _context.BankAccount.Update(bankAccount);
        }
    }
}