using Telegram.Bot;
using System.Threading.Tasks;
using ScorpPlusBackend.Models;
using System.Collections.Generic;

namespace ScorpPlusBackend.Services.Notifications
{
    /// <summary>
    /// Telegram notification system
    /// </summary>
    public class TelegramNotificator : INotificator
    {
        /// <summary>
        /// Telegram Bot Client for sending telegram notifications
        /// </summary>
        private readonly TelegramBotClient _bot;

        /// <summary>
        /// Templates for different types of notifications
        /// </summary>
        private readonly Dictionary<NotificationType, string> _templates = new Dictionary<NotificationType, string>
        {
            {NotificationType.General, "Topic: No Topic;\n_MESSAGE;"},
            {NotificationType.UserAuthorized, "Topic: User Authorization 🔐;\n_MESSAGE;"},
            {NotificationType.UserCreated, "Topic: User Creation ➕;\n_MESSAGE;"},
            {NotificationType.EmployeeEntered, "Topic: Employee Attendance ➡️🚪;\n_MESSAGE;"},
            {NotificationType.EmployeeExited, "Topic: Employee Attendance ⬅️🚪;\n_MESSAGE;"},
            {NotificationType.EmployeeNotInOffice, "WARNING: Employee Attendance ❌;\n_MESSAGE;"},
            {NotificationType.TemperatureIncreasedCritically, "CRITICALLY: Climate Changes 🌡;\n_MESSAGE;"},
            {NotificationType.TemperatureDecreasedCritically, "CRITICALLY: Climate Changes 🥶;\n_MESSAGE;"},
        };

        /// <summary>
        /// Telegram Notificator constructor
        /// </summary>
        /// <param name="telegramOptions">Configuration of telegram bot</param>
        public TelegramNotificator(Options.TelegramBotDto telegramOptions)
        {
            _bot = new TelegramBotClient(telegramOptions.Token);
        }

        /// <inheritdoc />
        public async Task Notify(User user, string message, NotificationType notificationType)
        {
            var telegramMessage = _templates[notificationType].Replace("_MESSAGE", message);
            await _bot.SendTextMessageAsync(user.TelegramChat.ChatId, telegramMessage);
        }
    }
}