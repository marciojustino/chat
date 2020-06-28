namespace Server.Entities
{
    public class Command
    {
        public static string HELP { get => "/help"; }
        public static string SEND_PRIVATE_MESSAGE { get => "/p"; }
        public static string LIST_ROOMS { get => "/rooms"; }
        public static string CREATE_ROOM { get => "/new_room"; }
        public static string ENTER_ROOM { get => "/enter_room"; }
        public static string EXIT { get => "/exit"; }
    }
}