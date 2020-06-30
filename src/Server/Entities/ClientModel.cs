using System;
using System.Net.Sockets;
using Chat.Server.Enum;

namespace Chat.Server.Entities
{
    public class ClientModel
    {
        public StatusEnum Status { get; set; }
        public CommandModel LastCommand { get; set; }
        public string Nickname { get; set; }
        public DateTime ConnectedAt { get; set; }
        public TcpClient Socket { get; set; }
    }
}