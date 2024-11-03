using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CounterStrikeHunter.Core.Service.Notification
{
    public class NotificationService : INotificationService
    {
        private ObservableCollection<Notification> _notifications = new ObservableCollection<Notification>();

        public event EventHandler UpdateEvent;

        public void Clear()
        {
            _notifications.Clear();

            UpdateEvent.Invoke(null, new EventArgs());
        }

        public ObservableCollection<Notification> GetAllNotifications()
        {
            return _notifications;
        }

        public void Update()
        {
            var itemsToRemove = new List<Notification>();

            foreach (var item in _notifications)
            {
                if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - item.Timestamp >= 4000)
                {
                    itemsToRemove.Add(item);
                }
            }

            foreach (var item in itemsToRemove)
            {
                _notifications.Remove(item);
            }
        }

        public void PutToPool(Notification notification)
        {
            _notifications.Add(notification);

            UpdateEvent.Invoke(null, new EventArgs());
        }
    }
}
