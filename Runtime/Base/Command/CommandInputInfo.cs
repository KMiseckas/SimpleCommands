// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using UnityEngine.Assertions;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Class that encapsulates information about the recently parsed command input string. Used to try execute an instance of <see cref="SCCommand"/> correctly,
    /// assuming the data parsed is correct.
    /// </summary>
    public class CommandInputInfo
    {
        /// <summary>
        /// The key of the command (command unique ID).
        /// </summary>
        private readonly string _CommandKey;

        /// <summary>
        /// The parameters of the passed in command string which will be later parsed into their correct Types based on function invoked by the <see cref="SCCommand"/> object.
        /// </summary>
        private readonly string[] _CommandParams;

        /// <summary>
        /// The target to invoke the <see cref="SCCommand"/> on.
        /// </summary>
        private readonly TargetInfo _TargetInfo;

        /// <summary>
        /// Create a new instance of <see cref="CommandInputInfo"/>.
        /// </summary>
        /// <param name="commandKey">The unique key (ID) of the command as parsed.</param>
        /// <param name="commandParams">The parameters as parsed from the input.</param>
        /// <param name="targetInfo">The <see cref="TargetInfo"/> as parsed.</param>
        internal protected CommandInputInfo(string commandKey, string[] commandParams, TargetInfo targetInfo = default)
        {
            Assert.IsNotNull(commandKey);
            Assert.IsNotNull(commandParams);

            _CommandKey = commandKey.ToLower();
            _CommandParams = commandParams;
            _TargetInfo = targetInfo;
        }

        /// <summary>
        /// Get the unique key of the command.
        /// </summary>
        internal protected string CommandKey => _CommandKey;

        /// <summary>
        /// Get the parameters of the command input as been parsed.
        /// </summary>
        internal protected string[] CommandParams => _CommandParams;

        /// <summary>
        /// Get the target info for the command.
        /// </summary>
        internal protected TargetInfo TargetInfo => _TargetInfo;
    }
}
