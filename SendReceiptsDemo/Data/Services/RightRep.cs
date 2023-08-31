using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Services
{
    public class RightRep : IRightRep
    {
        private SendReceiptContext _context;

        public RightRep(SendReceiptContext context)
        {
            _context = context;
        }

        public void AddRight(Right right)
        {
           _context.Rights.Add(right);
            _context.SaveChanges();
        }

        public void EditRight(Right right)
        {
            _context.Rights.Update(right);
            _context.SaveChanges();
        }

        public bool ExistRight(int rightId)
        {
            return _context.Rights.Any(r=> r.RightId == rightId);
        }

        public bool ExistRight(int righterId, int accountId)
        {
            return _context.Rights.Any(r => r.RighterId == righterId && r.AcountId == accountId);
        }

        public List<Right> GetAlRights()
        {
            return _context.Rights.OrderByDescending(r => r.RightId).ToList();
        }
        public List<Right> GetRightsForPages(int skip)
        {
            return _context.Rights.OrderByDescending(r => r.RightId).Skip(skip).Take(20).ToList();
        }

        public Right GetRightById(int rightId)
        {
            return _context.Rights.Find(rightId);
        }

        public void RemoveRight(Right right)
        {
            _context.Rights.Remove(right);
            _context.SaveChanges();
        }
        public void RemoveRight(int rightId)
        {
           var right=GetRightById(rightId);
            RemoveRight(right);
        }
    }
}
