using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Bem vindo ao cliente de chat room");

            int serverPort = 8888;
            if (args.Length > 0)
            {
                serverPort = int.Parse(args[0]);
            }

            var clientControl = new ClientControl();
            try
            {
                await clientControl.Connect(serverPort);
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
                clientControl.SendMessage(nickname);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't send your message|Error={0}", ex.Message);
                Exit();
            }

            Console.WriteLine("Your are registred as {0}. Joining #general", nickname);

            var msg = string.Empty;
            do
            {
                msg = Console.ReadLine();

                try
                {
                    clientControl.SendMessage(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can't send your message|Error={0}", ex.Message);
                }

            } while (msg != "/exit");

            Console.WriteLine("See you later. Bye!");
            Console.ReadKey();
            Exit();
        }

        static void Exit()
        {
            System.Environment.Exit(0);
        }
    }
}