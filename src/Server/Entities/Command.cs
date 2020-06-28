using Server.Enum;

namespace Server.Entities
{
    public class Command
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public CommandEnum Type { get; internal set; }
    }
}