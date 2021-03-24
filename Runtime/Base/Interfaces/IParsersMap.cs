using System;
using System.Collections.Generic;

namespace SimpleCommands
{
    public interface IParsersMap
    {
        bool GetParser(Type typeToParse, out Func<string, object> paramParser);
    }
}
