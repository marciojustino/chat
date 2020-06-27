using System.Collections.Generic;

namespace Server.Entities
{
    public class Room
    {
        public string Name { get; set; }
        public List<Client> Clients { get; set; }
    }
}