using Newtonsoft.Json.Linq;
using System.Collections;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GoogleScholarApi
{
    public class Program
    {
        static void Main(string[] args)
        {
            var client = new TelegramBotClient("Telegram_Bot_API");
            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message.Text != null)
            {
                if (message.Text == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введите запрос для Google Академия");
                    return;
                }

                String apiKey = "SerpApi_Google_Scholar_API";
                Hashtable ht = new Hashtable();
                ht.Add("engine", "google_scholar");
                ht.Add("q", $"{message.Text}");
                var requests = string.Empty;

                SerpApi.GoogleSearch search = new SerpApi.GoogleSearch(ht, apiKey);
                JObject data = search.GetJson();
                JArray results = (JArray)data["organic_results"];
                foreach (JObject result in results)
                {
                    var count = (int)result.First.First;
                    requests += $"{count + 1}." + " " + result["title"] + " " + result["link"] + Environment.NewLine;
                }
                await botClient.SendTextMessageAsync(message.Chat.Id, requests);
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Введи запрос...");
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}

