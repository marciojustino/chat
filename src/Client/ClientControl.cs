using System.Threading.Tasks;
using System.Text;
using System.Net;
using System;
using System.Net.Sockets;

namespace Client
{
    public class ClientControl
    {
        private TcpClient _clientSocket;
        private NetworkStream _serverStream;

        public ClientControl()
        {
            _clientSocket = new TcpClient();
        }

        public Task Connect(int serverPort)
        {
            return _clientSocket.ConnectAsync(IPAddress.Parse("127.0.0.1"), serverPort);
        }

        public void SendMessage(string msg)
        {
            _serverStream = _clientSocket.GetStream();
            var outStream = Encoding.ASCII.GetBytes(msg + "$");
            _serverStream.Write(outStream, 0, outStream.Length);
            _serverStream.Flush();

            Task.Factory.StartNew(HandleReceiveMsg);
        }

        private void HandleReceiveMsg()
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
                ShowMessage(dataFromServer);
            }
        }

        private void ShowMessage(string dataFromServer)
        {
            if (!string.IsNullOrWhiteSpace(dataFromServer))
            {
                Console.WriteLine("From server: {0}", dataFromServer);
            }
        }
    }
}