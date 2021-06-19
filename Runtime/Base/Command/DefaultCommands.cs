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

using UnityEditor;
using UnityEngine;

namespace SimpleCommands.Runtime.Base
{
    internal static class DefaultCommands
    {
        [SCCommand(commandKey: "SC_Commands_List", commandDescription: "Show all the available commands on the specified page.")]
        [SCCommand(commandKey: "List", commandDescription: "Show all the available commands on the specified page.")]
        [SCCommand(commandKey: "Help", commandDescription: "Show all the available commands on the specified page.")]
        public static void ShowAvailableCommands(int page = 0)
        {
            int maxCommandsPerPage = 15;

            string[] availableCommands = SCBase.Instance.CommandMap.GetAllCommandKeys();

            int commandCount = availableCommands.Length;
            int pages = 0;

            if (commandCount > maxCommandsPerPage)
            {
                int remainder = commandCount % maxCommandsPerPage;
                pages = ((commandCount - remainder) / maxCommandsPerPage); //Zero indexed
            }

            page = Mathf.Clamp(page, 0, pages);

            int startingElement = maxCommandsPerPage * page;
            int nextPage = Mathf.Min(page + 1, pages);

            int lastElement = nextPage == page ? commandCount : startingElement + maxCommandsPerPage;

            SCBase.OutConsole("");
            SCBase.OutConsole("------------------------------------------------------------------------------------------");

            for (int i = startingElement; i < lastElement; i++)
            {
                SCCommand command = null;

                if (!SCBase.Instance.CommandMap.TryGetCommand(availableCommands[i], out command))
                {
                    SCBase.OutConsole($"Command [{availableCommands[i]}] could not be found in the command map.", OutputType.WARNING);
                    continue;
                }

                string keyOutput = command.CommandKey;

                if (!command.Method.IsStatic)
                {
                    keyOutput += $" [target]";

                }

                for (int j = 0; j < command.ParamInfo.Length; j++)
                {
                    keyOutput += $" <{command.ParamInfo[j].Type.Name}>";

                    if (command.ParamInfo[j].IsOptional)
                    {
                        keyOutput += $"(opt)";
                    }
                }

                SCBase.OutConsole($" - {keyOutput}: {command.CommandDesc}");
            }

            SCBase.OutConsole("------------------------------------------------------------------------------------------");
            SCBase.OutConsole($"Page: {page+1}/{pages+1}");
            SCBase.OutConsole("------------------------------------------------------------------------------------------");
            SCBase.OutConsole("");
        }

        [SCCommand(commandKey: "SC_Command_Help", commandDescription: "Display detailed help for a given command.")]
        public static void CommandHelp(string commandKey)
        {
            if (!SCBase.Instance.CommandMap.TryGetCommand(commandKey, out SCCommand command))
            {
                string[] commandKeySuggestion = SCBase.Instance.CommandSuggester.GetSuggestions(commandKey);

                if (commandKeySuggestion.Length > 0 && !string.IsNullOrWhiteSpace(commandKeySuggestion[0]))
                {
                    SCBase.OutConsole($"Command [{commandKey}] could not be found, did you mean [{commandKeySuggestion}]?", OutputType.WARNING);
                    CommandHelp(commandKeySuggestion[0]);
                }
                else
                {
                    SCBase.OutConsole($"Command [{commandKey}] or any similar commands could not be found.", OutputType.WARNING);
                }
            }
            else
            {
                string keyOutput = command.CommandKey;

                if (!command.Method.IsStatic)
                {
                    keyOutput += $" [target]";

                }

                for (int j = 0; j < command.ParamInfo.Length; j++)
                {
                    keyOutput += $" <{command.ParamInfo[j].Type.Name}:{command.Method.GetParameters()[j].Name}>";

                    if (command.ParamInfo[j].IsOptional)
                    {
                        keyOutput += $"(opt)";
                    }
                }

                SCBase.OutConsole("");
                SCBase.OutConsole("--Command Info----------------------------------------------------------------------------");
                SCBase.OutConsole($"Key: {keyOutput}", OutputType.INFO);
                SCBase.OutConsole($"Desc: {command.CommandDesc}", OutputType.INFO);
                SCBase.OutConsole("------------------------------------------------------------------------------------------");
                SCBase.OutConsole("");
            }
        }


        [SCCommand(commandKey: "SC_Quit", commandDescription: "Exit the game: Exits play mode if in Editor. Exits the application if in standalone build.")]
        private static void QuitPlay()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        [SCCommand(commandKey: "SC_Time_Scale", commandDescription: "Set the games time scale.", buildTarget: BuildTarget.DEVELOPMENT_ONLY)]
        private static void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
    }
}