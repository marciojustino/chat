using System;
using System.Net;
using System.Net.Sockets;
using Server.Enum;

namespace Server.Entities
{
    public class Client
    {
        public StatusEnum Status { get; set; }
        public Command LastCommand { get; set; }
        public string Nickname { get; set; }
        public DateTime ConnectedAt { get; set; }
        public TcpClient Socket { get; set; }
    }
}