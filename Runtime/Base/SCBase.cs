// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using System;
using System.Collections.Generic;
using SimpleCommands.Runtime.Base;
using UnityEngine;
using UnityEngine.Serialization;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace SimpleCommands
{
    /// <summary>
    /// Base abstract class that handles the overall console / command system. Acts as a singleton and does not get destroyed on scene changes.
    /// This only works with the new Unity Input system and requires a component of <see cref="PlayerInput"/>.
    /// </summary>
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class SCBase : MonoBehaviour
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
        /// Should the input field auto focus back onto the display so input can be entered again after a command is issued.
        /// </summary>
        [FormerlySerializedAs("auto_focus_post_command")]
        [SerializeField]
        private bool _AutoFocusPostCommand = true;

#if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// Instance of <see cref="PlayerInput"/> component for input.
        /// </summary>
        protected PlayerInput _Input;
#endif

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
        protected CommandMap _CommandMapRef;

        /// <summary>
        /// Instance of the <see cref="ICommandInputParser"/> implementation used for trying to parse the command string that is visible in the console input field in order
        /// to try and execute.
        /// </summary>
        protected ICommandInputParser _CommandInputParser;

        /// <summary>
        /// Instance of the ITargetParser that holds the mapping between the string target type to the parser function that provides the objects which to target for command
        /// execution onto.
        /// </summary>
        private TargetParsersMap _CommandTargetParsers;

        /// <summary>
        /// The display which will show the output text.
        /// </summary>
        [SerializeField]
        private BaseCommandOutputDisplay _OutputDisplay;

        /// <summary>
        /// The display which will display and allow the user to input text.
        /// </summary>
        [SerializeField]
        private BaseCommandInputDisplay _InputDisplay;

        /// <summary>
        /// The display which will display and allow the user to selected suggested commands.
        /// </summary>
        [SerializeField]
        private BaseCommandSuggestionDisplay _SuggestionDisplay;

        /// <summary>
        /// Implementation instance of the <see cref="ITextSuggester"/>.
        /// </summary>
        private ITextSuggester _CommandSuggester;

        /// <summary>
        /// Current suggestions from the CommandSuggester.
        /// </summary>
        protected List<SCCommand> _CurrentCommandSuggestions;

        /// <summary>
        /// Is the console visible and usable.
        /// </summary>
        protected bool _IsVisible = false;

        /// <summary>
        /// Reference to the instance of this class.
        /// </summary>
        private static SCBase _Instance;

        /// <summary>
        /// Object to set <see langword="lock"/> on.
        /// </summary>
        private static readonly object _Lock = new object();

        /// <summary>
        /// Get instance of <see cref="ICommandMap"/>.
        /// </summary>
        public CommandMap CommandMapRef => _CommandMapRef;

        /// <summary>
        /// Get instance of <see cref="BaseCommandOutputDisplay"/>.
        /// </summary>
        public BaseCommandOutputDisplay OutputDisplay => _OutputDisplay;

        /// <summary>
        /// Get instance of <see cref="BaseCommandInputDisplay"/>.
        /// </summary>
        public BaseCommandInputDisplay InputDisplay => _InputDisplay;

        /// <summary>
        /// Get instance of BaseCommandSuggestionDisplay.
        /// </summary>
        public BaseCommandSuggestionDisplay SuggestionDisplay => _SuggestionDisplay;

        /// <summary>
        /// Get instance of <see cref="ITextSuggester"/>.
        /// </summary>
        public ITextSuggester CommandSuggester => _CommandSuggester;

        /// <summary>
        /// Get instance of <see cref="ITargetParser"/>.
        /// </summary>
        public TargetParsersMap CommandTargetParser => _CommandTargetParsers;

        /// <summary>
        /// Get or create an instance of this object depending on whether it already exists or not. Thread safe.
        /// </summary>
        public static SCBase Instance
        {
            get
            {
                lock (_Lock)
                {
                    if (_Instance == null)
                    {
                        _Instance = FindObjectOfType<SCBase>();
                    }

                    return _Instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            _CurrentCommandSuggestions = new List<SCCommand>();

            _Instance = this;

            _CommandMapRef = CreateCommandMap();
            _CommandInputParser = CreateCommandInputParser();
            _CommandSuggester = CreateCommandSuggester();
            _CommandTargetParsers = CreateCommandTargetParsers();

            //Populate the suggester with the collection of all the command keys.
            _CommandSuggester.AddCollection(_CommandMapRef.GetAllCommandKeys());

#if ENABLE_INPUT_SYSTEM
            _Input = GetComponent<PlayerInput>();
#endif

            DontDestroyOnLoad(this);

            HookActions();
#if ENABLE_INPUT_SYSTEM
            HookInputSystemActions();
#endif
        }

#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
        protected virtual void Update()
        {
            ActionLegacyInput();
        }

        /// <summary>
        /// Check and action the legacy input if the InputSystem package is not being used.
        /// </summary>
        protected virtual void ActionLegacyInput()
        {
            if (ToggleInputLegacy())
            {
                ToggleConsole();
            }

            if (_IsVisible)
            {
                if (IssueCommandInputLegacy())
                {
                    IssueCommand();
                }
                else if (PreviousCommandInputLegacy())
                {
                    PreviousCommand();
                }
                else if (NextCommandInputLegacy())
                {
                    NextCommand();
                }
            }
        }

        /// <summary>
        /// Returns whether the console can be toggled based on input detected.
        /// </summary>
        protected virtual bool ToggleInputLegacy()
        {
            return Input.GetKeyUp(KeyCode.BackQuote) && Input.GetKey(KeyCode.LeftControl);
        }

        /// <summary>
        /// Returns whether a command can be issued based on input detected.
        /// </summary>
        protected virtual bool IssueCommandInputLegacy()
        {
            return Input.GetKeyUp(KeyCode.Return);
        }

        /// <summary>
        /// Returns whether the previous command can be shown based on input detected.
        /// </summary>
        protected virtual bool PreviousCommandInputLegacy()
        {
            return Input.GetKeyUp(KeyCode.UpArrow);
        }

        /// <summary>
        /// Returns whether the next command can be shown based on input detected.
        /// </summary>
        protected virtual bool NextCommandInputLegacy()
        {
            return Input.GetKeyUp(KeyCode.DownArrow);
        }
#endif

        /// <summary>
        /// Create and return a new instance of <see cref="CommandMap"/> from which commands <see cref="SCCommand"/>s will be retrieved.
        /// </summary>
        /// <returns>New instance of <see cref="CommandMap"/>.</returns>
        protected CommandMap CreateCommandMap()
        {
            return new CommandMap(CreateParserMap());
        }

        /// <summary>
        /// Create a new instance of <see cref="TypeParsersMap"/>.
        /// </summary>
        /// <returns>New instance of <see cref="TypeParsersMap"/>.</returns>
        protected virtual TypeParsersMap CreateParserMap()
        {
            return new TypeParsersMap();
        }

        /// <summary>
        /// Create a new implementation instance of <see cref="ICommandInputParser"/>.
        /// </summary>
        /// <returns>New instance of <see cref="ICommandInputParser"/> implementation.</returns>
        protected virtual ICommandInputParser CreateCommandInputParser()
        {
            return new CommandInputParser();
        }

        /// <summary>
        /// Create a new implementation instance of <see cref="ITextSuggester"/>.
        /// </summary>
        /// <returns>New instance of <see cref="ITextSuggester"/> implementation.</returns>
        protected virtual ITextSuggester CreateCommandSuggester()
        {
            return new CommandSuggester();
        }

        /// <summary>
        /// Create an implementation instance of the ITargetParser that holds the mapping between the string target type to the parser function that provides the objects 
        /// which to target for command execution onto.
        /// </summary>
        /// <returns>New instance of <see cref="ITargetParser"/> implementation.</returns>
        protected virtual TargetParsersMap CreateCommandTargetParsers()
        {
            return new TargetParsersMap();
        }

        /// <summary>
        /// Hook required actions.
        /// </summary>
        protected virtual void HookActions()
        {
            BaseCommandInputDisplay.InputChangedEvent += OnCommandInputTextChanged;
            BaseCommandSuggestionDisplay.SelectedCommandSuggestionEvent += OnSuggestedCommandSelected;
        }

#if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// Hook required input to actions.
        /// </summary>
        protected virtual void HookInputSystemActions()
        {
            BindAction(OnToggleConsoleInput, "Toggle");
            BindAction(OnIssueCommandInput, "Issue");
            BindAction(OnPreviousCommandInput, "Previous");
            BindAction(OnNextCommandInput, "Next");

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
        /// Logic to run when Toggle the console on/off input is detected.
        /// </summary>
        protected virtual void OnToggleConsoleInput(InputAction.CallbackContext obj)
        {
            ToggleConsole();
        }
#endif

        /// <summary>
        /// Toggle the console on/off.
        /// </summary>
        protected virtual void ToggleConsole()
        {
            SetVisible(!_IsVisible);
        }

        /// <summary>
        /// Set the console display objects as visible or hidden.
        /// </summary>
        /// <param name="isVisible">Whether the displays should be visible.</param>
        protected virtual void SetVisible(bool isVisible)
        {
            _IsVisible = isVisible;

            _InputDisplay.SetVisible(isVisible);
            _OutputDisplay.SetVisible(isVisible);
            _SuggestionDisplay.SetVisible(isVisible);

            if (_InputDisplay.IsVisible)
                _InputDisplay.Focus();
        }

        /// <summary>
        /// Issue a command with the current input field text as command input.
        /// </summary>
        public void IssueCommand()
        {
            CommandInputInfo commandInputInfo = null;

            string inputString = _InputDisplay.GetInputString();

            if (!_CommandInputParser.TryParseCommandInput(inputString, out commandInputInfo))
                return;

            OutConsole(inputString, OutputType.FROM_INPUT);

            _CommandHistory.AddFirst(inputString);
            _CurrentlyDisplayedCommand = _CommandHistory.First;

            if (_CommandHistory.Count > _CommandHistoryCap)
            {
                _CommandHistory.RemoveLast();
            }

            if (!_CommandMapRef.TryGetCommand(commandInputInfo.CommandKey, out SCCommand command))
            {
                OutConsole($"Command `{commandInputInfo.CommandKey}` not found.", OutputType.WARNING);
            }
            else
            {
                if (command.TryExecute(CommandTargetParser, commandInputInfo.CommandParams, out string failOutput, commandInputInfo.TargetInfo))
                {
                    OutConsole($"Executed command `{commandInputInfo.CommandKey}`.", OutputType.SUCCESS);
                }
                else
                {
                    OutConsole(failOutput, OutputType.ERROR);
                }
            }

            _InputDisplay.OverrideInputString("");

            if (_AutoFocusPostCommand)
                _InputDisplay.Focus();
        }

#if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// On Issue a command input detected.
        /// </summary>
        protected void OnIssueCommandInput(InputAction.CallbackContext obj)
        {
            if (!_IsVisible) return;

            IssueCommand();
        }
#endif

        /// <summary>
        /// Set the input display field to show the previous command from the command history list.
        /// </summary>
        protected void PreviousCommand()
        {
            if (_CurrentlyDisplayedCommand == null)
                return;

            _InputDisplay.OverrideInputString(_CurrentlyDisplayedCommand.Value);

            LinkedListNode<string> temp = _CurrentlyDisplayedCommand;
            _CurrentlyDisplayedCommand = _CurrentlyDisplayedCommand.Next == null ? temp : _CurrentlyDisplayedCommand.Next;

            if (_AutoFocusPostCommand)
                _InputDisplay.Focus();
        }

#if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// On previous command input detected.
        /// </summary>
        protected void OnPreviousCommandInput(InputAction.CallbackContext obj)
        {
            if (!_IsVisible) return;

            PreviousCommand();
        }
#endif

        /// <summary>
        /// Set the input display field to show the next command from the command history list.
        /// </summary>
        protected void NextCommand()
        {
            if (_CurrentlyDisplayedCommand == null)
                return;

            string commandString = "";

            if (_CurrentlyDisplayedCommand.Previous != null)
            {
                _CurrentlyDisplayedCommand = _CurrentlyDisplayedCommand.Previous;
                commandString = _CurrentlyDisplayedCommand.Value;
            }

            _InputDisplay.OverrideInputString(commandString);

            if (_AutoFocusPostCommand)
                _InputDisplay.Focus();
        }

#if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// On next command input detected.
        /// </summary>
        protected void OnNextCommandInput(InputAction.CallbackContext obj)
        {
            if (!_IsVisible) return;

            NextCommand();
        }
#endif

        /// <summary>
        /// Auto complete the input field with the passed in string argument.
        /// </summary>
        /// <param name="targetString">What to fill the text field with.</param>
        protected virtual void AutoCompleteInputField(string targetString)
        {
            _InputDisplay.OverrideInputString(targetString);
        }

        /// <summary>
        /// Logic to run once the input text has changed.
        /// </summary>
        /// <param name="input">Text it has change to.</param>
        protected virtual void OnCommandInputTextChanged(string input)
        {
            string[] inputSplit = input.Split(new char[] { ' ' });

            if (inputSplit.Length > 1) return;

            string[] stringCommandSuggestions = _CommandSuggester.GetSuggestions(input);

            _CurrentCommandSuggestions.Clear();

            for (int i = 0; i < stringCommandSuggestions.Length; i++)
            {
                if (_CommandMapRef.TryGetCommand(stringCommandSuggestions[i], out SCCommand command))
                {
                    _CurrentCommandSuggestions.Add(command);
                }
            }

            _SuggestionDisplay.SetSuggestedCommands(_CurrentCommandSuggestions);
        }

        /// <summary>
        /// Logic to run once a suggestion has been selected from the suggestion display.
        /// </summary>
        /// <param name="command">Command that has been selected.</param>
        protected virtual void OnSuggestedCommandSelected(SCCommand command)
        {
            InputDisplay.OverrideInputString(command.CommandKey);
            InputDisplay.Focus();
        }

        /// <summary>
        /// Output text to the console for rendering.
        /// </summary>
        /// <param name="output">Output to display.</param>
        public static void OutConsole(string output, OutputType outputType = OutputType.NONE)
        {
            Instance._OutputDisplay.Output(output, outputType);
        }
    }
}
