using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Net.Sockets;

namespace Server.Services
{
    public class ClientHandle
    {
        private static TcpClient _clientSocket;
        private static string _clNo;
        private static Hashtable _clientsList;

        public void Start(TcpClient clientSocket, string clNo, Hashtable clientsList)
        {
            _clientSocket = clientSocket;
            _clNo = clNo;
            _clientsList = clientsList;

            Task.Factory.StartNew(DoChat);
        }

        private void DoChat()
        {
            var requestCount = 0;

            while (true)
            {
                try
                {
                    requestCount++;
                    var networkStream = _clientSocket.GetStream();
                    int receiveBufferSize = _clientSocket.ReceiveBufferSize;
                    var bytesFrom = new byte[receiveBufferSize];
                    
                    networkStream.Read(bytesFrom, 0, receiveBufferSize);
                    var dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    var idxEndStream = dataFromClient.IndexOf("$");
                    dataFromClient = dataFromClient.Substring(0, Math.Max(idxEndStream, 0));
                    Console.WriteLine("From client {0}:", _clNo, dataFromClient);

                    Server.Broadcast(dataFromClient, _clNo, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}