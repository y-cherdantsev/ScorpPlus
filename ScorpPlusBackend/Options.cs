using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ScorpPlusBackend
{
    /// <summary>
    /// Configuration options of an application
    /// </summary>
    public static class Options
    {
        /// <summary>
        /// Jwt service options
        /// </summary>
        public static Jwt JwtOptions { get; set; }

        /// <summary>
        /// Telegram notificator bot options
        /// </summary>
        public static TelegramBotDto TelegramBot { get; set; }

        /// <summary>
        /// Email notificator options
        /// </summary>
        public static MailingServerDto MailingServer { get; set; }

        /// <summary>
        /// Jwt service options
        /// </summary>
        public class Jwt
        {
            /// <summary>
            /// Application issuer
            /// </summary>
            public string Issuer { get; set; }

            /// <summary>
            /// Application users
            /// </summary>
            public string Audience { get; set; }

            /// <summary>
            /// Application key
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// Token lifetime in minutes
            /// </summary>
            public int Lifetime { get; set; }

            /// <summary>
            /// Key encryption module
            /// </summary>
            public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }

        /// <summary>
        /// TelegramBot service options
        /// </summary>
        public class TelegramBotDto
        {
            /// <summary>
            /// Bot token
            /// </summary>
            public string Token { get; set; }
        }

        /// <summary>
        /// Email service options
        /// </summary>
        public class MailingServerDto
        {
            /// <summary>
            /// Host
            /// </summary>
            public string Host { get; set; }

            /// <summary>
            /// Port
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// Username
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// Password
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// MailName
            /// </summary>
            public string MailName { get; set; }

            /// <summary>
            /// MailAddress
            /// </summary>
            public string MailAddress { get; set; }
        }
    }
}