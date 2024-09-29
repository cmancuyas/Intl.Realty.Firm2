using Intl.Realty.Firm.Models;

namespace Intl.Realty.Firm.Service.IService
{
    public interface INotificationService
    {
        public Notification Notify(string message, string title, NotificationType notificationType = NotificationType.success);
    }
}
