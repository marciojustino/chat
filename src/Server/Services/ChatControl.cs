using System.Collections.Generic;
using Server.Enum;

namespace Server.Services
{
    public class ChatControl
    {
        private readonly List<string> _commands;

        public ChatControl()
        {
            _commands = new List<string>
            {
                CommandEnum.CREATE_ROOM,
                CommandEnum.ENTER_ROOM,
                CommandEnum.EXIT,
                CommandEnum.HELP,
                CommandEnum.LIST_ROOMS,
                CommandEnum.SEND_PRIVATE_MESSAGE
            };
        }

        public string HandleCommand(string dataFromClient)
        {
            if (!_commands.Contains(dataFromClient))
            {
                return null;
            }

            foreach (var command in _commands)
            {
                if (command.IndexOf(dataFromClient) == 0)
                {
                    return command;
                }
            }

            return null;
        }
    }
}