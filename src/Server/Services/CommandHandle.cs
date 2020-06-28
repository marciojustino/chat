using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using Server.Entities;
using Server.Enum;

namespace Server.Services
{
    public class CommandHandle
    {
        private readonly List<Command> _commands;
        private List<string> _rooms;

        public CommandHandle()
        {
            _commands = new List<Command>
            {
                new Command { Name = "/new", Description="Criar nova sala de bate papo", Example="/new {room}", Type=CommandEnum.New},
                new Command { Name = "/enter", Description="Entrar na sala de bate papo", Example="/enter {room}", Type=CommandEnum.Enter},
                new Command { Name = "/exit", Description="Sair do bate papo", Example="/exit", Type=CommandEnum.Exit},
                new Command { Name = "/help", Description="Lista comandos disponíveis", Example="/help", Type=CommandEnum.Help},
                new Command { Name = "/list", Description="Lista todas as salas disponíveis", Example="/list", Type=CommandEnum.List},
                new Command { Name = "@", Description="Envia mensagem direta para usuário", Example="@{nickname} hello!", Type=CommandEnum.Direct},
                new Command { Name = "/p", Description="Envia mensagem privada para um usuário", Example="/p @{nickname}", Type=CommandEnum.Pvt},
            };
        }

        public Command ExtractCommand(string dataFromClient)
        {
            var match = Regex.Match(dataFromClient, @"^(\/\w+|@)");
            var foundCommand = _commands.Find(c => c.Name.Equals(match.Value));
            if (foundCommand != null)
            {
                return foundCommand;
            }
            else
            {
                return null;
            }
        }

        internal void ExecuteCommand(Command command, string sendMessage, Client client)
        {
            if (command.Type == CommandEnum.Pvt)
            {
                var matchDestiny = Regex.Match(sendMessage, @"@\w+");
                var destinyNickName = matchDestiny.Value.Replace("@", string.Empty);
                sendMessage = sendMessage.Replace(command.Name, string.Empty)
                                         .Replace(destinyNickName, string.Empty)
                                         .Trim();
                Server.SendPvtMessage(sendMessage, client.Nickname, destinyNickName);
            }

            if (command.Type == CommandEnum.Direct)
            {
                var matchDestiny = Regex.Match(sendMessage, @"@\w+");
                var destinyNickName = matchDestiny.Value.Replace("@", string.Empty);
                sendMessage = sendMessage.Replace(command.Name, string.Empty)
                                         .Replace(destinyNickName, string.Empty)
                                         .Trim();
                Server.Broadcast(sendMessage, client.Nickname, destinyNickName, true);
            }

            if (command.Type == CommandEnum.Exit)
            {
                Server.Disconnect(client);
                Server.Broadcast($"{client.Nickname} exit", null, false);
                return;
            }

            if (command.Type == CommandEnum.Help)
            {
                SendHelpCommand(client);
                return;
            }
        }

        private void SendHelpCommand(Client client)
        {
            var sb = new StringBuilder();
            sb.AppendLine("*** Lista de commandos");
            foreach (var cmd in _commands)
            {
                sb.AppendLine($"Comando: {cmd.Name}");
                sb.AppendLine($"Exemplo de uso: {cmd.Example}");
                sb.AppendLine($"Descrição: {cmd.Description}");
                sb.AppendLine();
            }
            Server.SendMessage(sb.ToString(), client.Socket);
        }
    }
}