using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Chat.Server.Entities;
using Chat.Server.Enum;
using Chat.Server.Services;

namespace Chat.Server
{
    public class Server
    {
        private readonly int _port;
        private readonly IPAddress _ipAddress;
        private static Hashtable _clientsList;
        private readonly TcpListener _serverSocket;
        private readonly CommandHandle _chatControl;

        public Server(IPAddress hostAddress, int port)
        {
            _port = port;
            _ipAddress = hostAddress;
            _clientsList = new Hashtable();
            _serverSocket = new TcpListener(_ipAddress, _port);
            _chatControl = new CommandHandle();
        }

        public void Start()
        {
            _serverSocket.Start();
            Console.WriteLine($"Chat server start listening on //{_ipAddress}:{_port}");

            while (true)
            {
                var clientSocket = _serverSocket.AcceptTcpClient();

                var networkStream = clientSocket.GetStream();
                int receiveBufferSize = clientSocket.ReceiveBufferSize;
                var bytesFrom = new byte[receiveBufferSize];

                networkStream.Read(bytesFrom, 0, receiveBufferSize);
                var dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                int idxEndStream = dataFromClient.IndexOf("$");
                dataFromClient = dataFromClient.Substring(0, Math.Max(idxEndStream, 0));

                if (NicknameExists(dataFromClient))
                {
                    SendMessage("Sorry, the nickname takeuser is already taken. Please choose a different one:", clientSocket);
                    clientSocket.Client.Disconnect(false);
                    clientSocket.Close();
                }
                else
                {
                    var client = new ClientModel
                    {
                        ConnectedAt = DateTime.Now,
                        Nickname = dataFromClient,
                        Socket = clientSocket,
                        Status = StatusEnum.ValidNickname
                    };
                    _clientsList.Add(dataFromClient, client);

                    SendMessage($"*** You are registered as {client.Nickname}. Joining #general.", clientSocket);
                    Broadcast($"{client.Nickname} joined", client.Nickname, false);

                    Console.WriteLine($"{client.Nickname} connected");

                    var clientHandle = new ClientHandle();
                    clientHandle.Start(client, _clientsList);
                }
            }
        }

        public static void Disconnect(ClientModel client)
        {
            if (_clientsList.ContainsKey(client.Nickname))
            {
                _clientsList.Remove(client.Nickname);
                client.Socket.Client.Disconnect(false);
                client.Socket.Close();
            }
        }

        private bool NicknameExists(string dataFromClient)
        {
            return _clientsList.ContainsKey(dataFromClient);
        }

        public static void Broadcast(string msg, string senderNickname, bool isFromClient)
        {
            foreach (DictionaryEntry clientKeyValue in _clientsList)
            {
                var client = clientKeyValue.Value as ClientModel;
                var newMsg = isFromClient
                    ? $"{senderNickname} says: {msg}"
                    : $"{msg}";

                SendMessage(newMsg, client.Socket);
            }
        }

        public static void Broadcast(string msg, string senderNickname, string destinyNickName, bool isFromClient)
        {
            foreach (DictionaryEntry clientKeyValue in _clientsList)
            {
                var client = clientKeyValue.Value as ClientModel;
                SendMessage($"{senderNickname} says to {destinyNickName}: {msg}", client.Socket);
            }
        }

        public static void SendPvtMessage(string msg, string senderNickname, string destinyNickname)
        {
            var deliveryTo = _clientsList[destinyNickname] as ClientModel;
            SendMessage($"{senderNickname} says privately to {destinyNickname}: {msg}", deliveryTo.Socket);
        }

        public static void SendMessage(string msg, TcpClient clientSocket)
        {
            var broadcastBytes = Encoding.ASCII.GetBytes($"{msg}$");
            var deliveryStream = clientSocket.GetStream();
            clientSocket.ReceiveBufferSize = broadcastBytes.Length;
            deliveryStream.Write(broadcastBytes, 0, broadcastBytes.Length);
            deliveryStream.Flush();
        }
    }
}