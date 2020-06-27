using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Net.Sockets;

namespace Server.Services
{
    public class ClientHandle
    {
        private TcpClient _clientSocket;
        private string _clNo;
        private Hashtable _clientsList;

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
            byte[] bytesFrom;
            string dataFromClient = null;

            while (true)
            {
                try
                {
                    requestCount++;
                    bytesFrom = new byte[10025];
                    var networkStream = _clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, _clientSocket.ReceiveBufferSize);
                    dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine("From client {0}:", _clNo, dataFromClient);
                    
                    ServerHandler.Broadcast(dataFromClient, _clNo, true);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}