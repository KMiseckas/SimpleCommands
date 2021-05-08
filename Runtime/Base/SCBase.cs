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
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace SimpleCommands
{
    public abstract class SCBase : MonoBehaviour
    {
        private const string ACTION_MAP_NAME = "Console";

        [FormerlySerializedAs("history_size_commands")]
        [SerializeField]
        [Min(1)]
        private int _CommandHistoryCap = 20;

        private PlayerInput _Input;

        private LinkedList<string> _CommandHistory = new LinkedList<string>();

        private LinkedListNode<string> _CurrentlyDisplayedCommand;

        private ICommandMap _CommandMap;

        private IParsersMap _ParsersMap;

        private ICommandInputParser _CommandInputParser;

        [SerializeField]
        private BaseCommandOutputDisplay _OutputPanel;

        [SerializeField]
        private BaseCommandInputDisplay _InputPanel; 

        private static SCBase _Instance;

        private static object _Lock = new object();

        public IParsersMap ParsersMap { get => _ParsersMap; set => _ParsersMap=value; }
        public ICommandMap CommandMap { get => _CommandMap; set => _CommandMap=value; }

        public static SCBase Instance
        {
            get
            {
                lock(_Lock)
                {
                    if(_Instance == null)
                    {
                        _Instance = FindObjectOfType<SCBase>();
                    }

                    return _Instance;
                }
            }
        }

        private void Awake()
        {
            if(_Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            _Instance = this;

            _ParsersMap = CreateParsersMap();
            _CommandMap = CreateCommandMap();
            _CommandInputParser = CreateCommandInputParser();

            _Input = GetComponent<PlayerInput>();

            DontDestroyOnLoad(this);

            HookInput();
        }

        internal protected abstract IParsersMap CreateParsersMap();

        internal protected abstract ICommandMap CreateCommandMap();

        internal protected abstract ICommandInputParser CreateCommandInputParser();

        /// <summary>
        /// Hook any input.
        /// </summary>
        private void HookInput()
        {
            BindAction(ToggleConsole, "Toggle");
            BindAction(IssueCommand, "Issue");
            BindAction(PreviousCommand, "Previous");
            BindAction(NextCommand, "Next");
        }

        /// <summary>
        ///     Gets an action from the player console input
        /// </summary>
        /// <param name="actionMapName"></param>
        /// <param name="actionName"></param>
        private InputAction GetAction(string actionMapName, string actionName)
        {
            InputActionMap actionMap = _Input.actions.FindActionMap(actionMapName, true);

            return actionMap.FindAction(actionName, true);
        }

        /// <summary>
        ///     Binds an action to a player input action
        /// </summary>
        /// <param name="actionMapName"></param>
        /// <param name="actionName"></param>
        /// <param name="callback"></param>
        private void BindAction(Action<InputAction.CallbackContext> callback, string actionName, string actionMapName = ACTION_MAP_NAME)
        {
            InputAction action = GetAction(actionMapName, actionName);

            action.performed += callback;
        }

        private void ToggleConsole(InputAction.CallbackContext obj)
        {
            _InputPanel.ToggleVisible();
            _OutputPanel.ToggleVisible();
        }

        public void IssueCommand()
        {
            IssueCommand(default);
        }

        private void IssueCommand(InputAction.CallbackContext obj)
        {
            CommandInputInfo commandInputInfo = null;

            string inputString = _InputPanel.GetInputString();

            if(!_CommandInputParser.TryParseCommandInput(inputString, out commandInputInfo))
                return;

            _CommandHistory.AddFirst(inputString);
            _CurrentlyDisplayedCommand = _CommandHistory.First;

            if(_CommandHistory.Count > _CommandHistoryCap)
            {
                _CommandHistory.RemoveLast();
            }

            if(!_CommandMap.GetCommand(commandInputInfo.CommandKey, out SCCommand command))
            {
                AddConsoleOutput($"Command `{commandInputInfo.CommandKey}` not found.");
            }
            else
            {
                if(command.TryExecute(commandInputInfo.CommandParams, out string output, commandInputInfo.TargetInfo))
                {
                    AddConsoleOutput($"Executed command `{commandInputInfo.CommandKey}`.");
                }
                else
                {
                    AddConsoleOutput(output);
                }
            }

            _InputPanel.OverrideInputString("");
        }

        private void PreviousCommand(InputAction.CallbackContext obj)
        {
            if(_CurrentlyDisplayedCommand == null)
                return;

            _InputPanel.OverrideInputString(_CurrentlyDisplayedCommand.Value);

            LinkedListNode<string> temp = _CurrentlyDisplayedCommand;
            _CurrentlyDisplayedCommand = _CurrentlyDisplayedCommand.Next == null ? temp : _CurrentlyDisplayedCommand.Next;
        }

        private void NextCommand(InputAction.CallbackContext obj)
        {
            if(_CurrentlyDisplayedCommand == null)
                return;

            string commandString = "";

            if(_CurrentlyDisplayedCommand.Previous != null)
            {
                _CurrentlyDisplayedCommand = _CurrentlyDisplayedCommand.Previous;
                commandString = _CurrentlyDisplayedCommand.Value;
            }

            _InputPanel.OverrideInputString(commandString);
        }

        public static void AddConsoleOutput(string output)
        {
            Instance._OutputPanel.Output(output);
        }
    }
}
