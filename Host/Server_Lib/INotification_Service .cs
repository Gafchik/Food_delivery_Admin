
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Client_Server_Library
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "INotification_Service" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(INotification_Service_Callback))]
    public interface INotification_Service
    {
        [OperationContract]
        int Connect(string name, string surname);

        [OperationContract]
        void Dissonnect(int Id);

        [OperationContract(IsOneWay = true)]
        void SendMsg(string chek_Id, string phone);
    }
}


public interface INotification_Service_Callback
{
    [OperationContract(IsOneWay = true)]
    void MsgCallback( string chek_Id, string phone);
}