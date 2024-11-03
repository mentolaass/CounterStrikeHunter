using System;
using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Service.Notification
{
    public interface INotificationService
    {
        event EventHandler UpdateEvent;

        void Update();

        void PutToPool(Notification notification);

        ObservableCollection<Notification> GetAllNotifications();

        void Clear();
    }
}
