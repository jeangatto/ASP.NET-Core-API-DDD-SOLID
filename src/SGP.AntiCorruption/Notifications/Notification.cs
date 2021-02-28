namespace SGP.AntiCorruption.Notifications
{
    public class Notification
    {
        public Notification(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; private set; }
        public string Message { get; private set; }
    }
}
