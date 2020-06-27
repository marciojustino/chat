﻿using System;
using Server.Services;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new ServerHandler(args[0]);
            server.Start();
            Console.WriteLine("Pressione qualquer tecla para finalizar o servidor");
            Console.ReadLine();
        }
    }
}