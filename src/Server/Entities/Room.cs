using System.Collections.Generic;

namespace Chat.Server.Entities
{
    public class Room
    {
        public string Name { get; set; }
        public List<ClientModel> Clients { get; set; }
    }
}