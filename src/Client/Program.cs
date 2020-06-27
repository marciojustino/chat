using System.Net.Sockets;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bem vindo ao cliente de chat room");

            Console.WriteLine("Informe a porta para conexão com o servidor:");
            var serverPort = Console.ReadLine();

            

            Console.WriteLine("Informe seu apelido:");
            var nickname = Console.ReadLine();

            var clientHandler = new ClientHandler();
        }
    }
}
