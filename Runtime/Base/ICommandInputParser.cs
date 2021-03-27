using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleCommands
{
    public interface ICommandInputParser
    {
        bool TryParseCommandInput(string commandInput, out CommandInputInfo commandInputInfo);
    }
}
