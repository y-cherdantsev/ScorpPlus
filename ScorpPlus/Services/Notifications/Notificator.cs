using ScorpPlus.Models;
using System.Threading.Tasks;

namespace ScorpPlus.Services.Notifications
{
    /// <summary>
    /// Notificator interface for implementing notification systems
    /// </summary>
    public interface INotificator
    {
        /// <summary>
        /// Sends notification to a given user
        /// </summary>
        /// <param name="user">User that will receive notification</param>
        /// <param name="message">Message text of notification</param>
        /// <param name="notificationType">Type of notification</param>
        public Task Notify(User user, string message, NotificationType notificationType);
    }

    public enum NotificationType
    {
        General,
        UserAuthorized,
        UserCreated,
        EmployeeEntered,
        EmployeeExited,
        EmployeeNotInOffice,
        TemperatureIncreasedCritically,
        TemperatureDecreasedCritically
    }
}