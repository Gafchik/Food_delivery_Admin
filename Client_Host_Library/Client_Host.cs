﻿
using Client_Host_Library.MyServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client_Server_Library
{
   public class Client_Host : INotification_ServiceCallback
    {
        public int Id { get; private set; }
        public bool IsConnectd { get; private set; } // проверка состояние подключения      
        private Notification_ServiceClient client;
        
        public void Connect(string name, string surname)
        {
            if (!IsConnectd)
            {
                client = new Notification_ServiceClient(new System.ServiceModel.InstanceContext(this));
               Id = client.Connect(name,surname);
                IsConnectd = true;
            }
        } 
        public void Disconnect()
        {
            if (IsConnectd)
            {
                client.Dissonnect(Id);
                client = null;
                IsConnectd = false;
            }
        }
       public void SendMsg(string chek_Id, string phone) => client.SendMsg(new Type_Msg(), chek_Id, phone);


        public void MsgCallback(Type_Msg type, string chek_Id, string phone)
        {           
            MessageBox.Show(type.msg_text + chek_Id +" Номер клиента: " +phone, type.msg_Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
