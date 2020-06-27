using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Server.Enum;

namespace Server.Services
{
    public class ServerHandler : IDisposable
    {
        private readonly int _port;
        private readonly IPAddress _ipAddress;
        private static Hashtable _clientsList;
        private readonly TcpListener _serverSocket;
        private readonly ChatControl _chatControl;

        public ServerHandler(int port)
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
            Console.WriteLine("Chat server start listening //{0}:{1}", _ipAddress, _port);
            var clientSocket = default(TcpClient);

            while (true)
            {
                clientSocket = _serverSocket.AcceptTcpClient();
                var networkStream = clientSocket.GetStream();
                var bytesFrom = new byte[clientSocket.ReceiveBufferSize];
                networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
                var dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                int idxEndStream = dataFromClient.IndexOf("$");
                dataFromClient = dataFromClient.Substring(0, Math.Max(idxEndStream, 0));
                _clientsList.Add(dataFromClient, clientSocket);
                Broadcast(string.Format("{0} Joined", dataFromClient), dataFromClient, false);
                Console.WriteLine("{0} joined chat room", dataFromClient);
                var client = new ClientHandle();
                client.Start(clientSocket, dataFromClient, _clientsList);
            }
        }

        public static void Broadcast(string msg, string uName, bool isFromClient)
        {
            foreach (DictionaryEntry client in _clientsList)
            {
                var broadcastSocket = client.Value as TcpClient;
                var broadcastStream = broadcastSocket.GetStream();
                var broadcastBytes = isFromClient
                    ? Encoding.ASCII.GetBytes(string.Format("{0} says: {1}", uName, msg))
                    : Encoding.ASCII.GetBytes(msg);
                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }

        public void Dispose()
        {
            foreach (DictionaryEntry client in _clientsList)
            {
                var socket = client.Value as TcpClient;
                if (socket != null && socket.Connected)
                {
                    socket.Close();
                    socket.Dispose();
                }
            }
            _serverSocket.Stop();
        }
    }
}