using System.Threading.Tasks;
using System.Text;
using System.Net;
using System;
using System.Net.Sockets;

namespace Client
{
    public class ClientHandler
    {
        private TcpClient _clientSocket;
        private NetworkStream _serverStream;

        public ClientHandler()
        {
            _clientSocket = new TcpClient();
            _serverStream = default(NetworkStream);
        }

        public void Connect(int serverPort)
        {
            _clientSocket.Connect(IPAddress.Parse("127.0.0.1"), serverPort);
        }

        public void SendMessage(string msg)
        {
            _serverStream=_clientSocket.GetStream();
            var outStream = Encoding.ASCII.GetBytes(msg + "$");
            _serverStream.Write(outStream, 0, outStream.Length);
            _serverStream.Flush();

            Task.Factory.StartNew(GetMessage);
        }

        private void GetMessage()
        {
            while(true)
            {
                _serverStream = _clientSocket.GetStream();
                int bufferSize = 0;
                var inStream = new byte[10025];
                bufferSize=_clientSocket.ReceiveBufferSize;
                _serverStream.Read(inStream, 0, bufferSize);
            }
        }
    }
}