using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Repositories
{
    public interface IBankAccountRep
    {
        public List<BankAccount> GetAllBankAccounts();
        public List<BankAccount> GetAccountsForPages(int skip);
        public BankAccount GetBankAccountById(int bankAccountId);
        public BankAccount GetBankAccountByCustomerId(int customerId);
        public BankAccount GetAccountByAccountNumber(string accountNumber);
        public void AddBankAccount(BankAccount bankAccount);
        public void EditBankAccount(BankAccount bankAccount);
        public void RemoveBankAccount(BankAccount bankAccount);
        public void RemoveBankAccount(int bankAccountId);
        public bool ExistBankAccountNumber(string bankAccountNumber, int accountId = 0);
        public List<BankAccountVM> GetBankAccountsOfCustomer(int customerId,int mode);
        public bool ExistBankAccount(int bankAccountId);
        public int GetCustomerIdByAccountId(int accountId);
    }
}
