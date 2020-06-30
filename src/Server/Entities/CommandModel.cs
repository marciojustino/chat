using Chat.Server.Enum;

namespace Chat.Server.Entities
{
    public class CommandModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public CommandEnum Type { get; internal set; }
    }
}