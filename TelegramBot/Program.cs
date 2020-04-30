using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using ApiAiSDK;
using ApiAiSDK.Model;
namespace TelegramBot
{
    class Program
    {
        static TelegramBotClient Bot;
        static ApiAi apiAi;

        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("1184397844:AAGAgal4LFmlVED8e57WOzfsJF8dijUzl0A");
            AIConfiguration config = new AIConfiguration("0934e1155e6e47568170d22114a1dc2e", SupportedLanguage.Russian);
            apiAi = new ApiAi(config); // подключились к dialogflow через апи в конфиге
            Bot.OnMessage += BotOnMessageReceived; // ответ на сообщение юзера
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;

            var me = Bot.GetMeAsync().Result;

            Console.WriteLine($"{me.FirstName} <- this is my bot name");
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data; 
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} выбрал(-а):  {buttonText}");
            //throw new NotImplementedException();

            try
            {
                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы выбрали: {buttonText}");
            }

            catch
            { }
        
            }

        private static async void BotOnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            
            var message = e.Message;

            if (message == null || message.Type != MessageType.Text) 
                return;

            string name = $"{message.From.FirstName} {message.From.LastName}";

            Console.WriteLine($"{name} отправил(-а) сообщение: '{message.Text}'");

            switch (message.Text)

            {
                case "/start":
                string text =
            @" Список команд: 
 /start - запустить бота 
 /hello - поприветствовать
 /callback - вывод меню
 /keyboard - клавиатура";

                await Bot.SendTextMessageAsync(message.From.Id, text);
                    break;

                case "/callback": // для отправки меню 
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                         new [] // первая строка
                         {
       InlineKeyboardButton.WithUrl ("Github" ,    "Github.com"),
       InlineKeyboardButton.WithUrl ("Instagram" , "instagram.com"),
       InlineKeyboardButton.WithUrl ("Telegram" ,  "https://t.me/")
                     },

                         new [] // вторая строка
                         {
                             InlineKeyboardButton.WithCallbackData ("Point 1"),
                             InlineKeyboardButton.WithCallbackData ("Point 2"),
                         }
                    }) ;
                         await Bot.SendTextMessageAsync(message.From.Id, "Выберите пункт меню", replyMarkup : inlineKeyboard);

                    break;
case "/keyboard": // клавиатура
                    var replyKeyboard = new ReplyKeyboardMarkup(new [] 
                    {
                        new []
                        {
                            new KeyboardButton ("Профиль"),
                            new KeyboardButton ("О боте")
                        }, 
                         new []
                         {
                            new KeyboardButton ("Контакт"),
                            new KeyboardButton ("Геолокация")
                         }
                        
                    });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сообщение", replyMarkup: replyKeyboard);

                    break;
case "/hello":
                    string text1 =
                        @"Приветствую!";
                    await Bot.SendTextMessageAsync(message.From.Id, text1);
                    break;
                default:
                    var response = apiAi.TextRequest(message.Text); // ответ юзеру
                    string answer = response.Result.Fulfillment.Speech; // получаем ответ от диалогфлоу
                  if (answer == "")
                        answer = "Прости, моя твоя не понимать :( ";
    
                    await Bot.SendTextMessageAsync(message.From.Id , answer);
                    break;





            }
        }
    }
}
