using System.Collections.Generic;

namespace SGP.Shared.Notifications
{
    /// <summary>
    /// Padrão de Notificação.
    /// REF: https://martinfowler.com/eaaDev/Notification.html
    /// </summary>
    public abstract class Notifiable
    {
        private readonly List<Notification> _notifications;

        protected Notifiable()
        {
            _notifications = new List<Notification>();
        }

        /// <summary>
        /// Se a validação foi bem-sucedida.
        /// </summary>
        public bool IsValid => _notifications.Count == 0;

        /// <summary>
        /// A coleção de notificaçãos (erros).
        /// </summary>
        public IReadOnlyList<Notification> Notifications => _notifications;

        /// <summary>
        /// Adiciona uma notificação (erro) na coleação de notificações.
        /// </summary>
        /// <param name="key">Chave da notificação.</param>
        /// <param name="message">Mensagem da notificação.</param>
        public void AddNotification(string key, string message)
        {
            _notifications.Add(new Notification(key, message));
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public void AddNotifications(IEnumerable<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void AddNotifications(Notifiable notifiable)
        {
            AddNotifications(notifiable?.Notifications);
        }

        public void AddNotifications(params Notifiable[] notifiables)
        {
            foreach (var item in notifiables)
            {
                AddNotifications(item);
            }
        }

        /// <summary>
        /// Limpa a coleção de notificaçãos (erros).
        /// </summary>
        public void Clear() => _notifications.Clear();
    }
}