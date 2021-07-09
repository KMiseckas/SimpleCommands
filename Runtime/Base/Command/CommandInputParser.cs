// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using System;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleCommands.Runtime.Base;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// The default implementation of the <see cref="ICommandInputParser"/>. Parses the executed command input string into its parts and tries to figure out the
    /// expected outcome.<br/><br/>
    /// 
    /// Also handles the stripping and parsing the string that determine what the <see cref="TargetIDType"/> of the command should be (only if target is applicable 
    /// or defined within the string).
    /// </summary>
    public class CommandInputParser : ICommandInputParser
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool TryParseCommandInput(string commandInput, out CommandInputInfo commandInputInfo)
        {
            commandInputInfo = null;

            if (commandInput == null || commandInput == "")
                return false;

            StripTargetInfo(ref commandInput, out string targetInfoString);

            string[] splitCommand = Regex.Matches(commandInput, @"(['`\""])(?<value>.+?)\1|(?<value>[^ ]+)")
                .Cast<Match>()
                .Select(m => m.Groups["value"].Value)
                .ToArray();

            string commandKey = splitCommand[0];
            ParamVal[] paramParse = new ParamVal[0];
            TargetInfo targetInfo = ParseTargetStringData(targetInfoString);

            if (splitCommand.Length > 1)
            {
                paramParse = new ParamVal[splitCommand.Length - 1];

                for (int i = 1; i < splitCommand.Length; i++)
                {
                    if (IsCommandInvokeArg(splitCommand[i]))
                    {
                        string nestedCommandInput = splitCommand[i].TrimStart(CommandInvokePrefix());

                        paramParse[i - 1].Val = SCBase.Instance.IssueCommand(nestedCommandInput);
                        paramParse[i -1].IsParsed = true;
                    }
                    else
                    {
                        paramParse[i - 1].Val = splitCommand[i];
                    }
                }
            }

            commandInputInfo = new CommandInputInfo(commandKey, paramParse, targetInfo);

            return true;
        }

        /// <summary>
        /// Check if the argument starts with a command invoker denoting symbol.
        /// </summary>
        /// <param name="commandArg">The command argument to check.</param>
        /// <returns>True if this argument has to invoke a command to get the actual argument value.</returns>
        protected virtual bool IsCommandInvokeArg(string commandArg)
        {
            return commandArg.StartsWith(char.ToString(CommandInvokePrefix()));
        }

        /// <summary>
        /// The prefix required for a input text to be considered as a command invoke argument.
        /// </summary>
        protected virtual char CommandInvokePrefix()
        {
            return '@';
        }

        /// <summary>
        /// Try to find and strip any string that defines the target that this command should be actioned on.
        /// </summary>
        /// <param name="commandInputString">The whole string of the command input.</param>
        /// <param name="targetInfoString">The string which contains the data for the target info.</param>
        private void StripTargetInfo(ref string commandInputString, out string targetInfoString)
        {
            targetInfoString = "";

            Match match = Regex.Match(commandInputString, @"\[([^]])\]");

            if (!match.Success)
                return;

            commandInputString = Regex.Replace(commandInputString, @"\[([^]])\]", "");

            targetInfoString = match.Groups[1].Value;
        }

        /// <summary>
        /// Parse the string that contains the command target data.
        /// </summary>
        /// <param name="targetDataString"> The string which contains the target data.</param>
        /// <returns>Instance of object that contains the target info.</returns>
        private TargetInfo ParseTargetStringData(string targetDataString)
        {
            if (targetDataString.Equals(""))
                return default;

            TargetInfo result = new TargetInfo();

            string[] targetInfoElements = targetDataString.Split('=');

            string targetType = targetInfoElements[0].ToLower();
            result.IDType = targetType;

            if (targetInfoElements.Length <= 1)
                return result;

            string[] targetIDs = targetInfoElements[1].Split(',');

            if (targetIDs.Length == 0)
                return result;

            result.ID = targetIDs[0]; //TODO make this so we can take multiple IDs for the same type, currently we only use the first element in array of the ID strings.

            return result;

        }
    }
}
