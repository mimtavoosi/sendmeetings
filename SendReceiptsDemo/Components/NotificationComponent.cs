using Microsoft.AspNetCore.Mvc;
using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SendMeetingsDemo.Components
{
    public class NotificationComponent : ViewComponent
    {
        private INotificationRep _notificationRep;

        public NotificationComponent(INotificationRep notificationRep)
        {
            _notificationRep = notificationRep;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("/Views/Components/NotificationComponent.cshtml", _notificationRep.GetEnableNotification()?? new Notification());
        }
    }
}
