using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Repositories
{
    public interface IRightRep
    {
        public List<Right> GetAlRights();
        public List<Right> GetRightsForPages(int skip);
        public Right GetRightById(int rightId);
        public void AddRight(Right right);
        public void EditRight(Right right);
        public void RemoveRight(Right right);
        public void RemoveRight(int rightId);
        public bool ExistRight(int rightId);
        public bool ExistRight(int righterId, int accountId);
    }
}
