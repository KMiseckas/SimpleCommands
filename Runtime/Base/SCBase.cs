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
    /// <summary>
    /// Base abstract class that handles the overall console / command system. Acts as a singleton and does not get destroyed on scene changes.
    /// This only works with the new Unity Input system and requires a component of <see cref="PlayerInput"/>.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public abstract class SCBase : MonoBehaviour
    {
        /// <summary>
        /// Name of the input action map used for this system. Hard coded.
        /// </summary>
        private const string ACTION_MAP_NAME = "Console";

        /// <summary>
        /// The max number of commands to remember from the latest command.
        /// </summary>
        [FormerlySerializedAs("history_size_commands")]
        [SerializeField]
        [Min(1)]
        private int _CommandHistoryCap = 20;

        /// <summary>
        /// Instance of <see cref="PlayerInput"/> component for input.
        /// </summary>
        protected PlayerInput _Input;

        /// <summary>
        /// List containing the history of the most recent commands, capped at amount based on <see cref="_CommandHistoryCap"/>.
        /// </summary>
        protected LinkedList<string> _CommandHistory = new LinkedList<string>();

        /// <summary>
        /// The currently displayed command from the command history, can be <see langword="null"/> if not displaying a command that is from the history list.
        /// </summary>
        protected LinkedListNode<string> _CurrentlyDisplayedCommand;

        /// <summary>
        /// Instance of the <see cref="ICommandMap"/> implementation used for retrieving instances of <see cref="SCCommand"/>.
        /// </summary>
        protected ICommandMap _CommandMap;

        /// <summary>
        /// Instance of the <see cref="ICommandInputParser"/> implementation used for trying to parse the command string that is visible in the console input field in order
        /// to try and execute.
        /// </summary>
        protected ICommandInputParser _CommandInputParser;

        /// <summary>
        /// The panel display which will show the output text.
        /// </summary>
        [SerializeField]
        private BaseCommandOutputDisplay _OutputPanel;

        /// <summary>
        /// The panel display which will display and allow the user to input text.
        /// </summary>
        [SerializeField]
        private BaseCommandInputDisplay _InputPanel; 

        /// <summary>
        /// Reference to the instance of this class.
        /// </summary>
        private static SCBase _Instance;

        /// <summary>
        /// Object to set <see langword="lock"/> on.
        /// </summary>
        private static readonly object _Lock = new object();

        /// <summary>
        /// Get/Set instance of <see cref="ICommandMap"/>.
        /// </summary>
        public ICommandMap CommandMap { get => _CommandMap; set => _CommandMap=value; }

        /// <summary>
        /// Get or create an instance of this object depending on whether it already exists or not. Thread safe.
        /// </summary>
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

            _CommandMap = CreateCommandMap();
            _CommandInputParser = CreateCommandInputParser();

            _Input = GetComponent<PlayerInput>();

            DontDestroyOnLoad(this);

            HookInput();
        }

        /// <summary>
        /// Create and return a new instance of <see cref="ICommandMap"/> implementation from which commands <see cref="SCCommand"/>s will be retrieved.
        /// </summary>
        /// <returns>New instance of <see cref="ICommandMap"/> implementation.</returns>
        protected abstract ICommandMap CreateCommandMap();

        /// <summary>
        /// Create a new implementation instance of <see cref="ICommandInputParser"/>.
        /// </summary>
        /// <returns>New instance of <see cref="ICommandInputParser"/> implementation.</returns>
        protected abstract ICommandInputParser CreateCommandInputParser();

        /// <summary>
        /// Hook required input.
        /// </summary>
        private void HookInput()
        {
            BindAction(ToggleConsole, "Toggle");
            BindAction(IssueCommand, "Issue");
            BindAction(PreviousCommand, "Previous");
            BindAction(NextCommand, "Next");
        }

        /// <summary>
        /// Gets an action from the player console input.
        /// </summary>
        /// <param name="actionMapName">Name of the action map.</param>
        /// <param name="actionName">Name of the action.</param>
        private InputAction GetAction(string actionMapName, string actionName)
        {
            InputActionMap actionMap = _Input.actions.FindActionMap(actionMapName, true);

            return actionMap.FindAction(actionName, true);
        }

        /// <summary>
        /// Binds an action to a player input action.
        /// </summary>
        /// <param name="actionMapName">Name of the action map.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="callback">Action to invoke.</param>
        private void BindAction(Action<InputAction.CallbackContext> callback, string actionName, string actionMapName = ACTION_MAP_NAME)
        {
            InputAction action = GetAction(actionMapName, actionName);

            action.performed += callback;
        }

        /// <summary>
        /// Toggle the console on/off.
        /// </summary>
        private void ToggleConsole(InputAction.CallbackContext obj)
        {
            _InputPanel.ToggleVisible();
            _OutputPanel.ToggleVisible();
        }

        /// <summary>
        /// Issue a command with the current input field text as command input.
        /// </summary>
        public void IssueCommand()
        {
            IssueCommand(default);
        }

        /// <summary>
        /// Issue a command with the current input field text as command input.
        /// </summary>
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
                OutConsole($"Command `{commandInputInfo.CommandKey}` not found.");
            }
            else
            {
                if(command.TryExecute(commandInputInfo.CommandParams, out string output, commandInputInfo.TargetInfo))
                {
                    OutConsole($"Executed command `{commandInputInfo.CommandKey}`.");
                }
                else
                {
                    OutConsole(output);
                }
            }

            _InputPanel.OverrideInputString("");
        }

        /// <summary>
        /// Set the input display field to show the previous command from the command history list.
        /// </summary>
        private void PreviousCommand(InputAction.CallbackContext obj)
        {
            if(_CurrentlyDisplayedCommand == null)
                return;

            _InputPanel.OverrideInputString(_CurrentlyDisplayedCommand.Value);

            LinkedListNode<string> temp = _CurrentlyDisplayedCommand;
            _CurrentlyDisplayedCommand = _CurrentlyDisplayedCommand.Next == null ? temp : _CurrentlyDisplayedCommand.Next;
        }

        /// <summary>
        /// Set the input display field to show the next command from the command history list.
        /// </summary>
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

        /// <summary>
        /// Output text to the console for rendering.
        /// </summary>
        /// <param name="output">Output to display.</param>
        public static void OutConsole(string output)
        {
            Instance._OutputPanel.Output(output);
        }
    }
}
