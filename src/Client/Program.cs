using System;

namespace Client
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Bem vindo ao cliente de chat room");

            int serverPort = 8888;
            if (args.Length > 0)
            {
                serverPort = int.Parse(args[0]);
            }

            var clientHandler = new ClientHandle();
            try
            {
                await clientHandler.ConnectAsync(serverPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't connect to chat server|Error={0}", ex.Message);
                Exit();
            }

            try
            {
                Console.WriteLine("Welcome to our chat server. Please provide a nickname");
                var nickname = Console.ReadLine();
                await clientHandler.SendMessage(nickname);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't send your message|Error={0}", ex.Message);
                Exit();
            }
        }

        static void Exit()
        {
            Console.Write("Chat ends");
            Console.ReadLine();
            System.Environment.Exit(0);
        }
    }
}