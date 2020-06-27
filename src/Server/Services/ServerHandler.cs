using System.Threading.Tasks;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server.Services
{
    public class ServerHandler : IDisposable
    {
        private static Hashtable _clientsList;
        private readonly TcpListener _serverSocket;

        public ServerHandler(string port)
        {
            _clientsList = new Hashtable();
            _serverSocket = new TcpListener(IPAddress.Parse("127.0.0.1"), int.Parse(port));
        }

        public void Start()
        {
            _serverSocket.Start();
            Console.WriteLine("Chat server start");
            var clientSocket = default(TcpClient);

            while (true)
            {
                clientSocket = _serverSocket.AcceptTcpClient();

                var bytesFrom = new byte[10025];

                var networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
                var dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                _clientsList.Add(dataFromClient, clientSocket);

                Broadcast(string.Format("{0} Joined", dataFromClient), dataFromClient, false);

                Console.WriteLine("{0} joined chat room", dataFromClient);
                var client = new ClientHandle();
                client.Start(clientSocket, dataFromClient, _clientsList);
                clientSocket.Close();
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
            _serverSocket.Stop();
        }
    }
}