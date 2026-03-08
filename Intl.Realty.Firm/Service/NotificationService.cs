using Intl.Realty.Firm.Models;
using Intl.Realty.Firm.Service.IService;

namespace Intl.Realty.Firm.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IConfigurationService _configurationService;

        public NotificationService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }
        public Notification Notify(string message, string title, NotificationType notificationType = NotificationType.success)
        {
            var notification = new Notification
            {
                Message = message,
                Title = title,
                Icon = notificationType.ToString(),
                Type = notificationType.ToString(),
                Provider = _configurationService.GetNotificationProvider()
            };

            return notification;
        }
    }
}
