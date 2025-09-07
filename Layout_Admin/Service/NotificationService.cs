namespace Layout_Admin.Service
{
    public class NotificationService
    {
        public event Action<string>? OnNotificationReceived;

        public void Notify(string message)
        {
            OnNotificationReceived?.Invoke(message);
        }
    }
}

