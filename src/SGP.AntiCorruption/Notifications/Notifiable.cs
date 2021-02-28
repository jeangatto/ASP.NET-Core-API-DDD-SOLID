using System.Collections.Generic;

namespace SGP.AntiCorruption.Notifications
{
    public abstract class Notifiable
    {
        private readonly List<Notification> _notifications;

        protected Notifiable()
        {
            _notifications = new List<Notification>();
        }

        public bool IsValid => _notifications.Count == 0;
        public IReadOnlyCollection<Notification> Notifications => _notifications;

        public void AddNotification(string key, string message)
        {
            _notifications.Add(new Notification(key, message));
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotifications(IReadOnlyCollection<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(IList<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(ICollection<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(Notifiable item)
        {
            AddNotifications(item?.Notifications);
        }

        public void AddNotifications(params Notifiable[] items)
        {
            foreach (var item in items)
            {
                AddNotifications(item);
            }
        }

        public void Clear()
        {
            _notifications.Clear();
        }
    }
}