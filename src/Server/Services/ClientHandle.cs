using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using Server.Entities;

namespace Server.Services
{
    public class ClientHandle
    {
        private Client _client;
        private Hashtable _clientsList;

        public void Start(Client client, Hashtable clientsList)
        {
            _client = client;
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
                    var networkStream = _client.Socket.GetStream();
                    int receiveBufferSize = _client.Socket.ReceiveBufferSize;
                    var bytesFrom = new byte[receiveBufferSize];

                    networkStream.Read(bytesFrom, 0, receiveBufferSize);
                    var dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    var idxEndStream = dataFromClient.IndexOf("$");
                    dataFromClient = dataFromClient.Substring(0, Math.Max(idxEndStream, 0));

                    Console.WriteLine($"Message from client {_client.Nickname}: {dataFromClient}");

                    // handle chat commands when income
                    var chatControl = new CommandHandle();
                    var cmd = chatControl.ExtractCommand(dataFromClient);
                    if (cmd != null)
                    {
                        chatControl.ExecuteCommand(cmd, dataFromClient, _client);
                    }
                    else
                    {
                        Server.Broadcast(dataFromClient, _client.Nickname, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}