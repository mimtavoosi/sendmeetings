using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Repositories
{
    public interface INotificationRep
    {
        public List<Notification> GetAllNotifications();
        public List<Notification> GetNotificationsForPages(int skip);
        public Notification GetNotificationById(int notificationId);
        public Notification GetEnableNotification();
        public void AddNotification(Notification notification);
        public void EditNotification(Notification notification);
        public void RemoveNotification(Notification notification);
        public void RemoveNotification(int notificationId);
        public bool ExistNotification(int notificationId);
        public bool ExistEnableNotification();
        public bool CheckNotificationStatus(bool status, int notificationId =0);
    }
}
