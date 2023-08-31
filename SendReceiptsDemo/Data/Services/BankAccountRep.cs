using Microsoft.EntityFrameworkCore;
using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Services
{
    public class BankAccountRep:IBankAccountRep
    {
        private SendReceiptContext _context;
        public BankAccountRep(SendReceiptContext context)
        {
            _context = context;
        }

        public void AddBankAccount(BankAccount bankAccount)
        {
            _context.BankAccounts.Add(bankAccount);
            _context.SaveChanges();
        }

        public void EditBankAccount(BankAccount bankAccount)
        {
            _context.BankAccounts.Update(bankAccount);
            _context.SaveChanges();
        }

        public bool ExistBankAccount(int bankAccountId)
        {
            return _context.BankAccounts.Any(e => e.AcountId == bankAccountId);
        }

        public bool ExistBankAccountNumber(string bankAccountNumber,int accountId = 0)
        {
            if (accountId != 0) return _context.BankAccounts.Include(b => b.Customer).Any(b => b.AccountNumber == bankAccountNumber && b.AcountId != accountId);
            else return _context.BankAccounts.Include(b => b.Customer).Any(b=> b.AccountNumber == bankAccountNumber);
        }

        public List<BankAccount> GetAllBankAccounts()
        {
            return _context.BankAccounts.Include(b => b.Customer).OrderByDescending(b => b.AcountId).ToList();
        }

        public List<BankAccount> GetAccountsForPages(int skip)
        {
            return _context.BankAccounts.Include(b => b.Customer).OrderByDescending(b => b.AcountId).Skip(skip).Take(20).ToList();
        }

        public BankAccount GetBankAccountById(int bankAccountId)
        {
            return _context.BankAccounts.Include(b => b.Customer).SingleOrDefault(b=> b.AcountId==bankAccountId);
        }

        public List<BankAccountVM> GetBankAccountsOfCustomer(int customerId,int mode)
        {
            List<BankAccount> Accounts = _context.BankAccounts.Include(b => b.Customer).Where(b => b.Customer.MobileNumber == _context.Customers.Find(customerId).MobileNumber).ToList().DistinctBy(a => a.AcountId).ToList();
            switch (mode)
            {
                default:
                case 1:
                    List<Right> otherCustomers = _context.Rights.Where(r =>_context.Customers.SingleOrDefault(c=> c.CustomerId==r.RighterId).MobileNumber == _context.Customers.SingleOrDefault(c=> c.CustomerId == customerId).MobileNumber).ToList();
                    otherCustomers.ForEach(x => Accounts.AddRange(_context.BankAccounts.Include(b => b.Customer).Where(b => b.AcountId == x.AcountId).ToList()));
                    return Accounts.Select(b => new BankAccountVM()
                    {
                        BankAcountId = b.AcountId,
                        AccountText = b.AccountNumber + " - " + b.Customer.FullName
                    }).ToList();
               case 2:
                    return Accounts.Select(b => new BankAccountVM()
                    {
                        BankAcountId = b.AcountId,
                        AccountText = b.AccountNumber + " - " + b.Customer.FullName
                    }).ToList();
            }
           
        }

        public int GetCustomerIdByAccountId(int accountId)
        {
            return _context.BankAccounts.Where(b => b.AcountId == accountId).SingleOrDefault().CustomerId.Value;
        }

        public void RemoveBankAccount(BankAccount bankAccount)
        {
            _context.Rights.Where(r => r.AcountId == bankAccount.AcountId).ToList().ForEach(r => _context.Rights.Remove(r));
            _context.SaveChanges();
            _context.Meetings.Where(m => m.BankAcountId == bankAccount.AcountId).ToList().ForEach(m => _context.Meetings.Remove(m));
            _context.SaveChanges();
            _context.BankAccounts.Remove(bankAccount);
            _context.SaveChanges();
        }

        public void RemoveBankAccount(int bankAccountId)
        {
           BankAccount bankAccount =GetBankAccountById(bankAccountId);
            RemoveBankAccount(bankAccount);
        }

        public BankAccount GetAccountByAccountNumber(string accountNumber)
        {
            return _context.BankAccounts.Include(b => b.Customer).SingleOrDefault(b => b.AccountNumber == accountNumber);
        }

        public BankAccount GetBankAccountByCustomerId(int customerId)
        {
            return _context.BankAccounts.FirstOrDefault(b=> b.CustomerId == customerId);
        }
    }
}
