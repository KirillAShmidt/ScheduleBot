using ParcingLogic;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace TelegramScheduleBot
{
    internal class TeleApp
    { 
        private ITelegramBotClient bot = new TelegramBotClient("");
        private DbController db;

        public List<string> GetNamesList
        {
            get
            {
                var namesList = new List<string>();

                foreach (var item in db.Subjects.Select(x => x.GroupName))
                {
                    if (!namesList.Contains(item!.ToLower()))
                        namesList.Add(item.ToLower());
                }

                return namesList;
            }
        }

        internal TeleApp()
        {
            db = new DbController();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var day = DateTime.Now.DayOfWeek.GetRuName();

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;

                if(message.Text.ToLower() == "/options")
                    await botClient.SendTextMessageAsync(message.Chat, "Кнопки");

                if (message.Text.ToLower() == "/start")
                    await botClient.SendTextMessageAsync(message.Chat, "Введи название группы или используйте команду /groups, чтобы посмотреть список групп");

                if(message.Text.ToLower() == "/groups")
                {
                    var names = "";

                    foreach (var item in GetNamesList)
                        names += $"{item}, ";

                    await botClient.SendTextMessageAsync(message.Chat, names);
                }

                if (GetNamesList.Contains(message.Text.ToLower()))
                {
                    var text = $"{day}\n";
                    var subj = db.Subjects.Where(x => x.GroupName.ToLower() == message.Text.ToLower()).Where(x => x.DayOfWeek == day);

                    foreach(var item in subj)
                        text += $"--------------------------------------\n{item.SubjectName} \nКабинет: {item.Room} \nНачало: {item.Time}\n";

                    await botClient.SendTextMessageAsync(message.Chat, text);
                }
            }
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public void Start()
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            while (true)
            {
                var text = Console.ReadLine();

                if(text == "update")
                {
                    db.Update();
                }
            }
        }
    }
}
