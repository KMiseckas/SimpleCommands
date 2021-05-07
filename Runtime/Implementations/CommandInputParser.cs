// MIT License 
//
// Copyright (c) 2021 Klaudijus Miseckas 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions: 
//
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
// SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SimpleCommands
{
    public class CommandInputParser : ICommandInputParser
    {
        private Dictionary<string, TargetIDType> _IDTypeStringMap = new Dictionary<string, TargetIDType>();

        internal protected CommandInputParser()
        {
            _IDTypeStringMap.Add("tag", TargetIDType.Tag);
            _IDTypeStringMap.Add("t", TargetIDType.Tag);
            _IDTypeStringMap.Add("id", TargetIDType.InstanceID);
        }

        public bool TryParseCommandInput(string commandInput, out CommandInputInfo commandInputInfo)
        {
            commandInputInfo = null;

            if(commandInput == null || commandInput == "")
                return false;

            StripTargetInfo(ref commandInput, out string targetInfoString);

            string[] splitCommand = commandInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string commandKey = splitCommand[0];
            string[] paramParse = new string[0];
            TargetInfo targetInfo = ParseTargetStringData(targetInfoString);

            if(splitCommand.Length > 1)
            {
                paramParse = new string[splitCommand.Length - 1];

                for(int i = 1; i < splitCommand.Length; i++)
                {
                    paramParse[i - 1] = splitCommand[i];
                }
            }

            commandInputInfo = new CommandInputInfo(commandKey, paramParse, targetInfo);

            return true;
        }

        private void StripTargetInfo(ref string commandInputString, out string targetInfoString)
        {
            targetInfoString = "";

            Match match = Regex.Match(commandInputString, @"\[([^]]*)\]");

            if(!match.Success)
                return;

            commandInputString = Regex.Replace(commandInputString, @"\[([^]]*)\]", "");

            targetInfoString = match.Groups[1].Value;
        }

        private TargetInfo ParseTargetStringData(string targetDataString)
        {
            if(targetDataString.Equals(""))
                return default;

            string[] targetInfoElements = targetDataString.Split('=');

            string targetType = targetInfoElements[0].ToLower();

            if(targetInfoElements.Length <= 1)
                return default;

            string[] targetIDs = targetInfoElements[1].Split(',');

            if(targetIDs.Length == 0)
                return default;

            if(!_IDTypeStringMap.TryGetValue(targetType, out TargetIDType idType))
            {
                return default;
            }

            TargetInfo result = new TargetInfo();
            result.IDType = idType;
            result.ID = targetIDs[0]; //TODO make this so we can take multiple IDs for the same type, currently we only use the first element in array of the ID strings.

            return result;

        }
    }
}
