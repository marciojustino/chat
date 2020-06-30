using System.Threading.Tasks;
using System.Text;
using System.Net;
using System;
using System.Net.Sockets;

namespace Chat.Client
{
    public class Client
    {
        private TcpClient _clientSocket;
        private NetworkStream _serverStream;

        public Client()
        {
            _clientSocket = new TcpClient();
        }

        public Task Connect(IPAddress hostAddress, int serverPort)
        {
            return _clientSocket.ConnectAsync(hostAddress, serverPort);
        }

        public void SendMessage(string msg)
        {
            _serverStream = _clientSocket.GetStream();
            var outStream = Encoding.ASCII.GetBytes(msg + "$");
            _serverStream.Write(outStream, 0, outStream.Length);
            _serverStream.Flush();

            Task.Factory.StartNew(HandleReceivedMessage);
        }

        private void HandleReceivedMessage()
        {
            while (true)
            {
                _serverStream = _clientSocket.GetStream();
                var receiveBufferSize = _clientSocket.ReceiveBufferSize;
                var inStream = new byte[receiveBufferSize];
                _serverStream.Read(inStream, 0, receiveBufferSize);
                var dataFromServer = Encoding.ASCII.GetString(inStream);
                var idxEndStream = dataFromServer.IndexOf("$");
                dataFromServer = dataFromServer.Substring(0, Math.Max(idxEndStream, 0));

                // TODO: Handle close command here too

                ShowMessage(dataFromServer);
            }
        }

        private void ShowMessage(string dataFromServer)
        {
            if (!string.IsNullOrWhiteSpace(dataFromServer))
            {
                Console.WriteLine($">> {dataFromServer}");
            }
        }
    }
}