using System.Threading;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private static ClientControl _clientControl;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Bem vindo ao cliente de chat room");

            int serverPort = 8888;
            if (args.Length > 0)
            {
                serverPort = int.Parse(args[0]);
            }

            _clientControl = new ClientControl();
            try
            {
                await _clientControl.Connect(serverPort);
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

                if (msg == "/close")
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

            } while (msg != "/close");

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