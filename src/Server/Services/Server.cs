using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Server.Entities;
using Server.Enum;

namespace Server.Services
{
    public class Server
    {
        private readonly int _port;
        private readonly IPAddress _ipAddress;
        private static Hashtable _clientsList;
        private readonly TcpListener _serverSocket;
        private readonly ChatControl _chatControl;

        public Server(int port)
        {
            _port = port;
            _ipAddress = IPAddress.Parse("127.0.0.1");
            _clientsList = new Hashtable();
            _serverSocket = new TcpListener(_ipAddress, _port);
            _chatControl = new ChatControl();
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
                    ReplyToClient("Sorry, the nickname takeuser is already taken. Please choose a different one:", clientSocket);
                    clientSocket.Client.Disconnect(false);
                    clientSocket.Close();
                }
                else
                {
                    var client = new Client
                    {
                        ConnectedAt = DateTime.Now,
                        Nickname = dataFromClient,
                        Socket = clientSocket,
                        Status = StatusEnum.ValidNickname
                    };
                    _clientsList.Add(dataFromClient, client);

                    ReplyToClient($"*** You are registered as {client.Nickname}. Joining #general.", clientSocket);
                    Broadcast($"{client.Nickname} joined", client.Nickname, false);

                    Console.WriteLine($"{client.Nickname} connected");

                    var clientHandle = new ClientHandle();
                    clientHandle.Start(client, _clientsList);
                }
            }
        }

        private void ReplyToClient(string message, TcpClient clientSocket)
        {
            SendClientMessage(message, null, false, clientSocket);
        }

        private bool NicknameExists(string dataFromClient)
        {
            return _clientsList.ContainsKey(dataFromClient);
        }

        public static void Broadcast(string msg, string nickname, bool isFromClient)
        {
            foreach (DictionaryEntry clientKeyValue in _clientsList)
            {
                var client = clientKeyValue.Value as Client;
                SendClientMessage(msg, nickname, isFromClient, client.Socket);
            }
        }

        private static void SendClientMessage(string msg, string nickname, bool isFromClient, TcpClient clientSocket)
        {
            var deliveryStream = clientSocket.GetStream();

            var broadcastBytes = isFromClient
                ? Encoding.ASCII.GetBytes($"{nickname} says: {msg}$")
                : Encoding.ASCII.GetBytes($"{msg}$");

            clientSocket.ReceiveBufferSize = broadcastBytes.Length;
            deliveryStream.Write(broadcastBytes, 0, broadcastBytes.Length);
            deliveryStream.Flush();
        }
    }
}