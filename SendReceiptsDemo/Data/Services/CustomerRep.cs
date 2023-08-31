using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Services
{
    public class CustomerRep:ICustomerRep
    {
        private SendReceiptContext _context;
        public CustomerRep(SendReceiptContext context)
        {
            _context = context;
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void EditCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public bool ExistCustomer(int customerId)
        {
            return _context.Customers.Any(e => e.CustomerId == customerId);
        }

        public bool ExistMobileNumber(string mobileNumber, int customerId = 0)
        {
            if(customerId != 0) return _context.Customers.Any(c => c.MobileNumber == mobileNumber && c.CustomerId!= customerId);
            else return _context.Customers.Any(c=> c.MobileNumber == mobileNumber);
        }

        public bool ExistNationalCode(string nationalCode, int customerId = 0)
        {
            if (customerId != 0) return _context.Customers.Any(c => c.NationalCode == nationalCode && c.CustomerId != customerId);
            else return _context.Customers.Any(c => c.NationalCode == nationalCode);
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.OrderByDescending(c => c.CustomerId).ToList();
        }

        public List<Customer> GetCustomersForPages(int skip)
        {
            return _context.Customers.OrderByDescending(c => c.CustomerId).Skip(skip).Take(20).ToList();
        }

        public Customer GetCustomerById(int customerId)
        {
            return _context.Customers.Find(customerId);
        }

        public Customer GetCustomerByMobileNumber(string mobileNumber)
        {
            return _context.Customers.FirstOrDefault(c=>c.MobileNumber==mobileNumber);
        }

        public Customer GetCustomerByNationalCode(string nationalCode)
        {
            return _context.Customers.FirstOrDefault(c => c.NationalCode == nationalCode);
        }

        public void RemoveCustomer(Customer customer)
        {
            _context.Meetings.Where(m => m.CustomerId == customer.CustomerId).ToList().ForEach(m => _context.Meetings.Remove(m));
            _context.SaveChanges();
            _context.Rights.Where(r => r.RighterId == customer.CustomerId).ToList().ForEach(r => _context.Rights.Remove(r));
            List<BankAccount> theAccounts = _context.BankAccounts.Where(b => b.CustomerId == customer.CustomerId).ToList();
            foreach (Right right in _context.Rights.ToList())
            {
                foreach (BankAccount account in theAccounts)
                {
                    if (right.AcountId == account.AcountId)
                        _context.Rights.Remove(right);
                }
            }
            _context.SaveChanges();
            _context.BankAccounts.Where(b => b.CustomerId == customer.CustomerId).ToList().ForEach(b => _context.BankAccounts.Remove(b));
            _context.SaveChanges();
            _context.Messages.Where(m => m.CustomerId == customer.CustomerId).ToList().ForEach(m => _context.Messages.Remove(m));
            _context.SaveChanges();
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        public void RemoveCustomer(int customerId)
        {
            Customer customer = GetCustomerById(customerId);
            RemoveCustomer(customer);
        }

        public Customer GetCustomerByAccountId(int accountId)
        {
            int customerid  = (int)_context.BankAccounts.SingleOrDefault(c=> c.AcountId == accountId).CustomerId;
            return GetCustomerById(customerid);
        }

        public bool ExistVarizId(string varizId, int customerId = 0)
        {
            if (customerId != 0) return _context.Customers.Any(c => c.VarizId == varizId && c.CustomerId != customerId);
            else return _context.Customers.Any(c => c.VarizId == varizId);
        }

        public List<Customer> GetCustomersByMobileNumber(string mobileNumber)
        {
            return _context.Customers.Where(c=> c.MobileNumber == mobileNumber).ToList();
        }

        public List<Customer> GetCustomersByNationalCode(string nationalCode)
        {
            return _context.Customers.Where(c => c.NationalCode == nationalCode).ToList();
        }
    }
}
