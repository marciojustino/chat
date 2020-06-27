namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Services.Server(args.Length == 0 ? 8888 : int.Parse(args[0]));
            server.Start();
        }
    }
}
