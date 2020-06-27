using System;
using System.Net;
using System.Net.Sockets;
using Server.Enum;

namespace Server.Entities
{
    public class Client
    {
        public CommandEnum LastCommand { get; set; }
        public IPAddress IPAddress { get; set; }
        public string Nickname { get; set; }
        public DateTime ConnectedAt { get; set; }
        public TcpClient Socket { get; set; }
    }
}