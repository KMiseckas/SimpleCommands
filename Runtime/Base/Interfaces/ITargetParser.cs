using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCommands;
using UnityEngine;

public interface ITargetParser
{
    bool TryGetParser(string idType, out Func<SCCommand, string, object[]> targetObjectParser);
}
