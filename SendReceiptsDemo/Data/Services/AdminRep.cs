using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Services
{
    public class AdminRep:IAdminRep
    {
        private SendReceiptContext _context;
        public AdminRep(SendReceiptContext context)
        {
            _context = context;
        }

        public void AddAdmin(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }

        public bool CheckPassword(string userName,string password)
        {
           return _context.Admins.Any(x => x.UserName == userName && x.Password == password);
        }

        public void EditAdmin(Admin admin)
        {
            _context.Admins.Update(admin);
            _context.SaveChanges();
        }

        public bool ExistAdmin(int adminId)
        {
            return _context.Admins.Any(a => a.AdminId == adminId);
        }

        public bool ExistUserName(string userName, int adminId=0)
        {
            if (adminId != 0) return _context.Admins.Any(a => a.UserName == userName && a.AdminId != adminId);
            else return _context.Admins.Any(a => a.UserName == userName);
        }

        public Admin GetAdminById(int adminId)
        {
            return _context.Admins.Find(adminId);
        }

        public Admin GetAdminForLogin(string userName, string password)
        {
            return _context.Admins.SingleOrDefault(x => x.UserName == userName && x.Password == password);
        }

        public List<Admin> GetAllAdmins()
        {
            return _context.Admins.OrderByDescending(a => a.AdminId).ToList();
        }

        public List<Admin> GetAdminsForPages(int skip)
        {
            return _context.Admins.OrderByDescending(a => a.AdminId).Skip(skip).Take(20).ToList();
        }

        public void RemoveAdmin(Admin admin)
        {
            _context.Admins.Remove(admin);
            _context.SaveChanges();
        }

        public void RemoveAdmin(int adminId)
        {
           Admin admin = GetAdminById(adminId);
           RemoveAdmin(admin);
        }
    }
}
