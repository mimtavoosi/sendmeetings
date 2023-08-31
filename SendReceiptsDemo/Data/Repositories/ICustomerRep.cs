using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Repositories
{
    public interface ICustomerRep
    {
        public List<Customer> GetAllCustomers();
        public List<Customer> GetCustomersForPages(int skip);
        public List<Customer> GetCustomersByMobileNumber(string mobileNumber);
        public List<Customer> GetCustomersByNationalCode(string nationalCode);
        public Customer GetCustomerById(int customerId);
        public Customer GetCustomerByMobileNumber(string mobileNumber);
        public Customer GetCustomerByNationalCode(string nationalCode);
        public Customer GetCustomerByAccountId(int accountId);
        public void AddCustomer(Customer customer);
        public void EditCustomer(Customer customer);
        public void RemoveCustomer(Customer customer);
        public void RemoveCustomer(int customerId);
        public bool ExistMobileNumber(string mobileNumber, int customerId=0);
        public bool ExistNationalCode(string nationalCode, int customerId=0);
        public bool ExistVarizId(string varizId, int customerId = 0);
        public bool ExistCustomer(int customerId);
    }
}
