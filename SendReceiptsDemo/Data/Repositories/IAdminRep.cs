using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Repositories
{
    public interface IAdminRep
    {
        public List<Admin> GetAllAdmins();
        public List<Admin> GetAdminsForPages(int skip);
        public Admin GetAdminById(int adminId);
        public Admin GetAdminForLogin(string userName, string password);
        public void AddAdmin(Admin admin);
        public void EditAdmin(Admin admin);
        public void RemoveAdmin(Admin admin);
        public void RemoveAdmin(int adminId);
        public bool ExistUserName(string userName, int adminId = 0);
        public bool CheckPassword(string userName,string password);
        public bool ExistAdmin(int adminId);
    }
}
