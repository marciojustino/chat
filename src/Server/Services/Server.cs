using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

                _clientsList.Add(dataFromClient, clientSocket);

                Broadcast($"{dataFromClient} joined", dataFromClient, false);
                Console.WriteLine($"{dataFromClient} joined");

                var clientHandle = new ClientHandle();
                clientHandle.Start(clientSocket, dataFromClient, _clientsList);
            }
        }

        public static void Broadcast(string msg, string uName, bool isFromClient)
        {
            foreach (DictionaryEntry client in _clientsList)
            {
                var clientSocket = client.Value as TcpClient;
                var deliveryStream = clientSocket.GetStream();

                var broadcastBytes = isFromClient
                    ? Encoding.ASCII.GetBytes($"{uName} says: {msg}$")
                    : Encoding.ASCII.GetBytes($"{msg}$");

                clientSocket.ReceiveBufferSize = broadcastBytes.Length;
                deliveryStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                deliveryStream.Flush();
            }
        }
    }
}