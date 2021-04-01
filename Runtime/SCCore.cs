using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCommands
{
    public class SCCore : SCBase
    {
        internal protected override IParsersMap CreateParsersMap()
        {
            return new BaseParsersMap();
        }

        internal protected override ICommandMap CreateCommandMap()
        {
            return new CommandMap(ParsersMap);
        }

        internal protected override ICommandInputParser CreateCommandInputParser()
        {
            return new CommandInputParser();
        }
    }
}
