using System.Linq;
using System.Threading.Tasks;
using ScorpPlusBackend.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ScorpPlusBackend.Services
{
    /// <summary>
    /// Service that sends notification about some activities in scorp system
    /// </summary>
    public class NotificationService
    {
        /// <summary>
        /// Notification context
        /// </summary>
        private readonly NotificationContext _notificationContext;

        /// <summary>
        /// Telegram notification service
        /// </summary>
        private readonly TelegramNotificator _telegramNotificator;

        /// <summary>
        /// Constructor for notification service
        /// </summary>
        /// <param name="notificationContext">Notification context</param>
        public NotificationService(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
            _telegramNotificator = new TelegramNotificator(Options.TelegramBot);
        }

        /// <summary>
        /// Notifies given user
        /// </summary>
        /// <param name="userId">User to be notified</param>
        /// <param name="messageText">Text of a notification message</param>
        /// <param name="topic">Topic of a notification message</param>
        public async Task Notify(int userId, string messageText, string topic = "Without topic")
        {
            var user = _notificationContext.Users.Include(x => x.TelegramChat)
                .FirstOrDefault(x => x.Id == userId);
            if (user?.TelegramChat != null)
                await _telegramNotificator.Notify(user.TelegramChat.ChatId, messageText, topic);
        }

        /// <summary>
        /// Notifies users with given roles (For ex. all 'admin')
        /// </summary>
        /// <param name="roleCode">Role code to be notified</param>
        /// <param name="messageText">Text of a notification message</param>
        /// <param name="topic">Topic of a notification message</param>
        public void Notify(string roleCode, string messageText, string topic = "Without topic")
        {
            var users = _notificationContext.Users.Include(x => x.TelegramChat)
                .Where(user => user.Role.Code == roleCode);
            var chatIds = users.Where(x => x.TelegramChat != null).Select(user => user.TelegramChat.ChatId).ToList();
            chatIds.ForEach(async chatId => await _telegramNotificator.Notify(chatId, messageText, topic));
        }
    }
}