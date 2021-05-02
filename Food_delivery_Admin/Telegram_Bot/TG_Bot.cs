using Food_delivery_Admin;
using Food_delivery_library.About_orders;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;

namespace Food_delivery_library.Telegram_Bot
{

    public class UserTG
    {
        public long chatID { get; set; }
        public string phone { get; set; }
    }
    public class TG_Bot
    {
        private User_Repository user_repository = new User_Repository();
        private Blocked_users_Repository blocked_users_repository = new Blocked_users_Repository();
        private Products_Repository products_Repository = new Products_Repository();


        private Current_Orders_Repository current_order_repository = new Current_Orders_Repository();
        private Current_Chek_Repository current_CH_repository = new Current_Chek_Repository();
        private Completed_Chek_Repository completed_CH_repository = new Completed_Chek_Repository();
        private Completed_Orders_Repository completed_order_repository = new Completed_Orders_Repository();

        // private string token = "1479607829:AAG-h2ZKL84U2pxa-_6oEYTK00xlzdJsRbs";
        private string token = Resource1.TG_token;

        private List<UserTG> usersTG;

        private string def = "БОТ РАБОТАЕТ ТОЛЬКО В РАБОЧЕЕ ВРЕМЯ =)\n" + "Напиши мне :\n" +
                                    "1 ==> Узнать скидках на продукты =)\n" +
                                    "2 ==> Помотреть мои заказы =)\n" +
                                    "3 ==> Помотреть всю историю моих заказов =)\n";

        public TelegramBotClient client;
        public TG_Bot()
        {
            usersTG = new List<UserTG>();
            client = new TelegramBotClient(token) { Timeout = TimeSpan.FromSeconds(10) };
            client.OnMessage += Client_OnMessage;
            client.StartReceiving();
        }

        private async void Client_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg == null)
                return;
            if (!usersTG.Exists(i => i.chatID == e.Message.Chat.Id))
            {
                if (blocked_users_repository.GetColl().ToList().Exists(i => i.Blocked_user_Phone == e.Message.Text))
                {
                    await client.SendTextMessageAsync(e.Message.Chat.Id,
                  "Простите но вы заблокированы =(",
                  replyToMessageId: msg.MessageId);
                    return;
                }
                if (user_repository.GetColl().ToList().Exists(i => i.User_Phone == e.Message.Text))
                {

                    usersTG.Add(new UserTG
                    {
                        phone = e.Message.Text,
                        chatID = e.Message.Chat.Id
                    });

                    await client.SendTextMessageAsync(e.Message.Chat.Id,
                   "Такой номер есть\n теперь ты можешь меня спросить =)",
                   replyToMessageId: msg.MessageId);


                    await client.SendTextMessageAsync(e.Message.Chat.Id, def);
                }
                else
                {

                    await client.SendTextMessageAsync(e.Message.Chat.Id,
                       "БОТ РАБОТАЕТ ТОЛЬКО В РАБОЧЕЕ ВРЕМЯ =)\n" +
                       "Прости но сначала мне нужен номер указаный при регестрации =(",
                       replyToMessageId: msg.MessageId);
                }
            }
            else
            {
                switch (msg.Text)
                {
                    case "1":
                        await client.SendTextMessageAsync(e.Message.Chat.Id, GetDiscount(), replyToMessageId: msg.MessageId);
                        break;
                    case "2":
                        await client.SendTextMessageAsync(e.Message.Chat.Id, GetCurrentChek(e), replyToMessageId: msg.MessageId);
                        break;
                    case "3":
                        await client.SendTextMessageAsync(e.Message.Chat.Id, GetHistory(e), replyToMessageId: msg.MessageId);
                        break;
                    default:
                        await client.SendTextMessageAsync(e.Message.Chat.Id, def, replyToMessageId: msg.MessageId);
                        break;
                }
            }

        }
        private string GetDiscount()
        {
            string rezult = "";
            products_Repository.GetColl().ToList().FindAll(i => i.Product_Discount != 0)
                .ForEach(i => rezult += i.Product_Name + " ==> Скидка : " + i.Product_Discount + " %\n\n");
            return rezult + "\n" + def; ;
        }

        private string GetCurrentChek(Telegram.Bot.Args.MessageEventArgs e)
        {
            string rezult = "";
            var chek = current_CH_repository.GetColl().ToList()
                  .FindAll(i => i.Check_User_Phone == usersTG.Find(j => j.chatID == e.Message.Chat.Id).phone);
            foreach (var i in chek)
            {
                rezult += "Номер заказа : " + i.Check_Id + "\n" +
                                       "Ваш администарор : " + i.Check_Admin + "\n" +
                                       "Дата заказа : " + i.Check_Date + "\n" +
                                       "Спиок продуктов : \n";
                current_order_repository.GetColl().ToList()
                                      .FindAll(j => j.Order_Chek_Id == i.Check_Id).ForEach(j => rezult += j.Order_Products_Name + "\n");
                rezult += "Общая стоимость заказа : " + i.Check_Final_Price + "\n";
                rezult += "Статус заказа : Bыполняется\n";
                rezult += "=======================\n";
            }
            return rezult + "\n" + def;
        }

        private string GetHistory(Telegram.Bot.Args.MessageEventArgs e)
        {
            string rezult = "";
            var chek = completed_CH_repository.GetColl().ToList()
                  .FindAll(i => i.Check_User_Phone == usersTG.Find(j => j.chatID == e.Message.Chat.Id).phone);
            foreach (var i in chek)
            {
                rezult += "Номер заказа : " + i.Check_Id + "\n" +
                                       "Ваш администарор : " + i.Check_Admin + "\n" +
                                       "Дата заказа : " + i.Check_Date + "\n" +
                                       "Спиок продуктов : \n";
                completed_order_repository.GetColl().ToList()
                                      .FindAll(j => j.Order_Chek_Id == i.Check_Id).ForEach(j => rezult += j.Order_Products_Name + "\n");
                rezult += "Общая стоимость заказа : " + i.Check_Final_Price + "\n";
                rezult += "Статус заказа : Выполнено\n";
                rezult += "=======================\n";
            }
            return rezult + "\n" + def; ;
        }
    }
}

