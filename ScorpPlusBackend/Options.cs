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
        /// Jwt service options
        /// </summary>
        public static Telegram TelegramBot { get; set; }

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
        public class Telegram
        {
            /// <summary>
            /// Bot token
            /// </summary>
            public string Token { get; set; }
        }
    }
}