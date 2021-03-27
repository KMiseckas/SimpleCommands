using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCommands
{
    public class SCCore : SCBase
    {
        protected override IParsersMap CreateParsersMap()
        {
            return new BaseParsersMap();
        }

        protected override ICommandMap CreateCommandMap()
        {
            return new CommandMap(ParsersMap);
        }

        protected override ICommandInputParser CreateCommandInputParser()
        {
            return new CommandInputParser();
        }
    }
}
