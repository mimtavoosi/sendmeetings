using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data.Services
{
    public class NotificationRep : INotificationRep
    {
        private SendReceiptContext _context;

        public NotificationRep(SendReceiptContext context)
        {
            _context = context;
        }

        public void AddNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }

        public void EditNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
        }

        public bool ExistNotification(int notificationId)
        {
            return _context.Notifications.Any(r=> r.NotificationId == notificationId);
        }

        public List<Notification> GetAllNotifications()
        {
            return _context.Notifications.OrderByDescending(r => r.NotificationId).ToList();
        }
        public List<Notification> GetNotificationsForPages(int skip)
        {
            return _context.Notifications.OrderByDescending(r => r.NotificationId).Skip(skip).Take(20).ToList();
        }

        public Notification GetNotificationById(int notificationId)
        {
            return _context.Notifications.Find(notificationId);
        }

        public void RemoveNotification(Notification notification)
        {
            _context.Notifications.Remove(notification);
            _context.SaveChanges();
        }
        public void RemoveNotification(int notificationId)
        {
           var notification=GetNotificationById(notificationId);
            RemoveNotification(notification);
        }

        public bool CheckNotificationStatus(bool status, int notificationId =0)
        {
            bool result = false;
            if (!status) result = true;
            else
            {
                if (notificationId != 0)
                {
                    result = !_context.Notifications.Any(n=> n.NotificationStatus =="فعال" && n.NotificationId != notificationId);
                }
                else
                {
                    result = !_context.Notifications.Any(n => n.NotificationStatus == "فعال");
                }
            }
            return result;
        }

        public Notification GetEnableNotification()
        {
            if (ExistEnableNotification())
            {
                return _context.Notifications.FirstOrDefault(n => n.NotificationStatus == "فعال");
            }
            else return null;
        }

        public bool ExistEnableNotification()
        {
            return _context.Notifications.Any(n => n.NotificationStatus == "فعال");
        }
    }
}
