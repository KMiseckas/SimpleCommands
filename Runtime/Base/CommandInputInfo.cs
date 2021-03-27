using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleCommands
{
    public class CommandInputInfo
    {
        private readonly string _CommandKey;
        private readonly string[] _CommandParams;
        private readonly TargetInfo _TargetInfo;

        internal protected CommandInputInfo(string commandKey, string[] commandParams, TargetInfo targetInfo = default)
        {
            Assert.IsNotNull(commandKey);
            Assert.IsNotNull(commandParams);

            _CommandKey = commandKey.ToLower();
            _CommandParams = commandParams;
            _TargetInfo = targetInfo;
        }

        internal protected string CommandKey => _CommandKey;
        internal protected string[] CommandParams => _CommandParams;
        internal protected TargetInfo TargetInfo => _TargetInfo;
    }
}
