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
        private readonly char[] _GenericEnclosureSymbols;

        private readonly char[] _TargetArgumentEnclosureSymbols;

        protected char[] GenericEnclosureSymbols => _GenericEnclosureSymbols;
        protected char[] TargetArgumentEnclosureSymbols => _TargetArgumentEnclosureSymbols;

        protected internal CommandInputParser()
        {
            _GenericEnclosureSymbols = CreateGenericEnclosureSymbols();
            _TargetArgumentEnclosureSymbols = CreateTargetArgumentDefinitionSymbols();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool TryParseCommandInput(string commandInput, out CommandInputInfo commandInputInfo)
        {
            commandInputInfo = null;

            if (commandInput == null || commandInput == "")
                return false;

            StripFirstTargetInfo(ref commandInput, out string targetInfoString);

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
        private void StripFirstTargetInfo(ref string commandInputString, out string targetInfoString)
        {
            targetInfoString = "";

            /*Match match = Regex.Match(commandInputString, @"\[([^\]]*)\]");

            if (!match.Success)
                return;

            string pattern = @"\[([^\]]*)\]";
            Regex regex = new Regex(pattern);

            commandInputString = regex.Replace(commandInputString, "", 1);

            targetInfoString = match.Groups[1].Value;*/

            bool hitGenericEnclosureSymbol = false;
            char nextSymbolToCheck = ' ';
            int targetSymbolTocheck = 0;

            int targetStart = -1;
            int targetEnd = -1;

            for (int i = 0; i < commandInputString.Length; i++)
            {
                if (targetSymbolTocheck != 1)
                {
                    if (hitGenericEnclosureSymbol ? nextSymbolToCheck.Equals(commandInputString[i]) : IsGenericEnclosureSymbol(commandInputString[i]))
                    {
                        hitGenericEnclosureSymbol = !hitGenericEnclosureSymbol;
                        nextSymbolToCheck = commandInputString[i];

                        continue;
                    }

                    if (hitGenericEnclosureSymbol)
                        continue;
                }

                if (commandInputString[i].Equals(TargetArgumentEnclosureSymbols[targetSymbolTocheck]))
                {
                    if (targetSymbolTocheck == 0)
                    {
                        targetSymbolTocheck = 1;
                        targetStart = i;
                    }
                    else if (targetSymbolTocheck == 1)
                    {
                        targetSymbolTocheck = 0;
                        targetEnd = i;

                        break;
                    }
                }
            }

            if (targetStart != -1 && targetEnd != -1)
            {
                int length = targetEnd - targetStart;

                targetInfoString = commandInputString.Substring(targetStart + 1, length - 1);
                commandInputString = commandInputString.Remove(targetStart, length + 1);
            }
        }

        protected bool IsGenericEnclosureSymbol(char symbol)
        {
            for (int i = 0; i < GenericEnclosureSymbols.Length; i++)
            {
                if (symbol.Equals(GenericEnclosureSymbols[i]))
                    return true;
            }

            return false;
        }

        //instancetest [t=GameObject] {@command_two 50} 5.9

        /// <summary>
        /// Get the symbols that denotes the opening and closing of any argument.
        /// </summary>
        /// <returns>Symbol as char that starts and ends the argument.</returns>
        protected virtual char[] CreateGenericEnclosureSymbols()
        {
            return new char[] { '\'', '\"', '`' };
        }

        /// <summary>
        /// Get the symbols that denotes the start and end of a target as an argument.
        /// </summary>
        /// <returns>Symbol as char that starts and ends the argument with a target, where char[0] is the opening symbol.</returns>
        protected virtual char[] CreateTargetArgumentDefinitionSymbols()
        {
            return new char[2] { '[', ']' };
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
