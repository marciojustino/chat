using System.Linq;
using System.Net;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace Chat.Client
{
    class Program
    {
        private static Client _clientControl;

        static async Task Main(string[] args)
        {
            Console.Write("--args: [");
            foreach (var arg in args)
            {
                Console.Write($"{arg},");
            }
            Console.WriteLine("]");

            Console.WriteLine("Welcome to chat room. Glad to see you :=D");

            if (args.Length != 2)
            {
                Console.WriteLine("Provide the host and port args to connect to chat server. <IP> <PORT>");
                Exit();
            }

            IPAddress hostAddress = null;
            try
            {
                hostAddress = IPAddress.Parse(args[0]);
            }
            catch
            {
                try
                {
                    var host = Dns.GetHostEntry(args[0]);
                    hostAddress = host.AddressList[0];
                }
                catch
                {
                    Console.WriteLine("Host IP Address invalid! Provide a valid IP for server.");
                    Exit();
                }
            }

            int port = 0;
            try
            {
                port = int.Parse(args[1]);
            }
            catch
            {
                Console.WriteLine("Port number invalid! Provide a valid TCP port for server.");
                Exit();
            }

            _clientControl = new Client();
            try
            {
                await _clientControl.Connect(hostAddress, port);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't connect to chat server|Error={0}", ex.Message);
                Exit();
            }

            Console.WriteLine("Welcome to our chat server. Please provide a nickname");
            var nickname = Console.ReadLine();

            try
            {
                _clientControl.SendMessage(nickname);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't define your nickname|Error={0}", ex.Message);
                Exit();
            }

            var msg = string.Empty;
            do
            {
                msg = Console.ReadLine();

                if (msg == "/exit")
                {
                    break;
                }

                try
                {
                    _clientControl.SendMessage(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can't send your message|Error={0}", ex.Message);
                }

            } while (msg != "/exit");

            Console.WriteLine("See you later. Bye!");
            Thread.Sleep(3000);
            Exit();
        }

        static void Exit()
        {
            System.Environment.Exit(0);
        }
    }
}