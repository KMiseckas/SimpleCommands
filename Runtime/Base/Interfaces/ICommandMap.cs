using System.Collections.Generic;

namespace SimpleCommands
{
    public interface ICommandMap
    {
        bool GetCommand(string commandKey, out SCCommand command);
    }
}
