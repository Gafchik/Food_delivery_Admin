
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Client_Server_Library
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Notification_Service" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] // мод для сервиса один сервис на всех клиентов 
    public class Notification_Service : INotification_Service
    {
        List<ServerUser> ServerUsers = new List<ServerUser>();
        int nextId = 1;
        public int Connect(string name, string surname)
        {
            ServerUser user = new ServerUser
            {
                Id = nextId,
                Name = name,
                Surname =surname,
                operationContext = OperationContext.Current
            };
            nextId++;
           
            ServerUsers.Add(user);
            return user.Id;
        }

        public void Dissonnect(int Id)
        {
            var user = ServerUsers.FirstOrDefault(i => i.Id == Id);
            if (user != null)
            {
                //SendMsg(user.Name + " покинул к чат",0); //уведомление о ушедшем пользователе
                ServerUsers.Remove(user);
            }
        }

        public void SendMsg( string chek_Id, string phone)
        {
            ServerUsers[0].operationContext.GetCallbackChannel<INotification_Service_Callback>().MsgCallback( chek_Id, phone);
        }
    }
}
