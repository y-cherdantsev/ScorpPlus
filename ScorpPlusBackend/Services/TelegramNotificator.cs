using Telegram.Bot;
using System.Threading.Tasks;

namespace ScorpPlusBackend.Services
{
    public class TelegramNotificator
    {
        private readonly TelegramBotClient _bot;

        public TelegramNotificator(Options.Telegram telegramOptions)
        {
            _bot = new TelegramBotClient(telegramOptions.Token);
        }

        public async Task Notify(long chatId, string message, string topic) =>
            await _bot.SendTextMessageAsync(chatId, $"Topic: {topic};\n{message}");
    }
}