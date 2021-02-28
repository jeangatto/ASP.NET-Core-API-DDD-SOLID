namespace SGP.Shared.Notifications
{
    public sealed class Notification
    {
        public Notification(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; }
        public string Message { get; }

        public override string ToString()
        {
            return $"{Key}: {Message}";
        }
    }
}
