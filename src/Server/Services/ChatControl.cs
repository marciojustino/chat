using System.Collections.Generic;
using Server.Entities;

namespace Server.Services
{
    public class ChatControl
    {
        private readonly List<string> _commands;

        public ChatControl()
        {
            _commands = new List<string>
            {
                Command.CREATE_ROOM,
                Command.ENTER_ROOM,
                Command.EXIT,
                Command.HELP,
                Command.LIST_ROOMS,
                Command.SEND_PRIVATE_MESSAGE
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