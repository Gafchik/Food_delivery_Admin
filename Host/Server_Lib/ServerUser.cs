using System.ServiceModel;
namespace Client_Server_Library
{
    public  class ServerUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public OperationContext operationContext { get; set; }
    }
}
