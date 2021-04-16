namespace ScorpPlusAdminDashboard
{
    /// <summary>
    /// Configuration options of an application
    /// </summary>
    public static class Options
    {
        /// <summary>
        /// Telegram notificator bot options
        /// </summary>
        public static TelegramBotDto TelegramBot { get; set; }

        /// <summary>
        /// Email notificator options
        /// </summary>
        public static MailingServerDto MailingServer { get; set; }

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