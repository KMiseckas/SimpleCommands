using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCommands;
using SimpleCommands.Attributes;
using UnityEditor;
using UnityEngine;

namespace SimpleCommands
{
    internal static class DefaultCommands
    {
        [SCCommand(commandKey: "SC_Commands_List", commandDescription: "Show all the available commands on the specified page.")]
        public static void ShowAvailableCommands(int page = 0)
        {
            int maxCommandsPerPage = 15;

            string[] availableCommands = SCBase.Instance.CommandMap.GetAllCommandKeys();

            int commandCount = availableCommands.Length;
            int pages = 1;

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
            SCBase.OutConsole("------------------------------------------------------------------------------------------", OutputType.INFO);

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

            SCBase.OutConsole("------------------------------------------------------------------------------------------", OutputType.INFO);
            SCBase.OutConsole($"Page: {page}/{pages}", OutputType.INFO);
            SCBase.OutConsole("------------------------------------------------------------------------------------------", OutputType.INFO);
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
                SCBase.OutConsole("--Command Info----------------------------------------------------------------------------", OutputType.INFO);
                SCBase.OutConsole($"Key: {keyOutput}", OutputType.INFO);
                SCBase.OutConsole($"Desc: {command.CommandDesc}", OutputType.INFO);
                SCBase.OutConsole("------------------------------------------------------------------------------------------", OutputType.INFO);
                SCBase.OutConsole("");
            }
        }


        [SCCommand(commandKey:"SC_Quit", commandDescription:"Exit the game: Exits play mode if in Editor. Exits the application if in standalone build.")]
        private static void QuitPlay()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        [SCCommand(commandKey:"SC_Time_Scale", commandDescription:"Set the games time scale.", buildTarget:BuildTarget.DEVELOPMENT_ONLY)]
        private static void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
    }
}