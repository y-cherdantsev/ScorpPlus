using System;
using System.Linq;
using ScorpPlus.Contexts;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ScorpPlus.Services.Notifications
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
        /// Email notification service
        /// </summary>
        private readonly EmailNotificator _emailNotificator;

        /// <summary>
        /// Constructor for notification service
        /// </summary>
        /// <param name="notificationContext">Notification context</param>
        /// <param name="telegramNotificator">Telegram notificator</param>
        /// <param name="emailNotificator">Mail notificator</param>
        public NotificationService(NotificationContext notificationContext, TelegramNotificator telegramNotificator, EmailNotificator emailNotificator)
        {
            _notificationContext = notificationContext;
            _telegramNotificator = telegramNotificator;
            _emailNotificator = emailNotificator;
        }

        /// <summary>
        /// Notifies given user
        /// </summary>
        /// <param name="userId">User to be notified</param>
        /// <param name="messageText">Text of a notification message</param>
        /// <param name="notificationType">Notification Type</param>
        public async Task Notify(int userId, string messageText,
            NotificationType notificationType = NotificationType.General)
        {
            var user = _notificationContext.Users.Include(x => x.TelegramChat)
                .FirstOrDefault(x => x.Id == userId);
            try
            {
                if (user?.TelegramChat != null)
                    await _telegramNotificator.Notify(user, messageText, notificationType);
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                if (user?.Email != null)
                    await _emailNotificator.Notify(user, messageText, notificationType);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Notifies users with given roles (For ex. all 'admin')
        /// </summary>
        /// <param name="roleCode">Role code to be notified</param>
        /// <param name="messageText">Text of a notification message</param>
        /// <param name="notificationType">Notification Type</param>
        public void Notify(string roleCode, string messageText,
            NotificationType notificationType = NotificationType.General)
        {
            var users = _notificationContext.Users.Include(x => x.TelegramChat)
                .Where(user => user.Role.Code == roleCode);
            try
            {
                var usersWithTelegram = users.Where(x => x.TelegramChat != null).ToList();
                usersWithTelegram.ForEach(async user =>
                    await _telegramNotificator.Notify(user, messageText, notificationType));
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                var usersWithEmail = users.Where(x => x.Email != null).ToList();
                usersWithEmail.ForEach(async user =>
                    await _emailNotificator.Notify(user, messageText, notificationType));
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}