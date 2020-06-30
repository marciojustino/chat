using System.Linq;
using System.Net;
using System;

namespace Chat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("--args: [");
            foreach (var arg in args)
            {
                Console.Write($"{arg},");
            }
            Console.WriteLine("]");

            if (args.Length != 2)
            {
                Console.WriteLine("You need specify host and port addresses to run server application");
                Environment.Exit(0);
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
                    Console.WriteLine("You need specify a valid IP address for host");
                    Environment.Exit(0);
                }
            }

            var server = new Server(hostAddress, int.Parse(args[1]));
            server.Start();
        }
    }
}
