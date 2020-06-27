using System.Threading.Tasks;
using System.Text;
using System.Net;
using System;
using System.Net.Sockets;

namespace Client
{
    public class ClientHandle : IDisposable
    {
        private TcpClient _clientSocket;
        private NetworkStream _serverStream;

        public ClientHandle()
        {
            _clientSocket = new TcpClient();
            _serverStream = default(NetworkStream);
        }

        public async Task ConnectAsync(int serverPort)
        {
            await _clientSocket.ConnectAsync(IPAddress.Parse("127.0.0.1"), serverPort);
        }

        public void Dispose()
        {
            _serverStream?.Close();
            _clientSocket?.Close();
        }

        public Task SendMessage(string msg)
        {
            _serverStream = _clientSocket.GetStream();
            var outStream = Encoding.ASCII.GetBytes(msg + "$");
            _serverStream.Write(outStream, 0, outStream.Length);
            _serverStream.Flush();

            return Task.Factory.StartNew(GetMessage);
        }

        private void GetMessage()
        {
            // while (true)
            // {
            _serverStream = _clientSocket.GetStream();
            int bufferSize = 0;
            bufferSize = _clientSocket.ReceiveBufferSize;
            var inStream = new byte[bufferSize];
            _serverStream.Read(inStream, 0, bufferSize);
            // }
        }
    }
}