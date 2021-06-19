using System.Collections.Generic;

namespace SimpleCommands.Runtime.Base
{
    public abstract class BaseCommandSuggestionDisplay : Display
    {
        public delegate void OnSelectedCommandSuggestionDelegate(SCCommand command);
        public static OnSelectedCommandSuggestionDelegate SelectedCommandSuggestionEvent;

        /// <summary>
        /// List of all the suggested commands that can be picked from for displaying.
        /// </summary>
        protected List<SCCommand> _SuggestedCommandList = new List<SCCommand>();

        /// <summary>
        /// Display the suggested commands.
        /// </summary>
        /// <param name="outputMessage">Suggestions as instances of command objects.</param>
        public virtual void SetSuggestedCommands(List<SCCommand> suggestedCommands)
        {
            _SuggestedCommandList.Clear();
            _SuggestedCommandList.AddRange(suggestedCommands);
        }

        /// <summary>
        /// Trigger the public selected command suggestion event: <see cref="SelectedCommandSuggestionEvent"/>.
        /// </summary>
        /// <param name="val">The selected command from available suggestions.</param>
        protected virtual void TriggerSelectedCommandSuggestion(SCCommand command)
        {
            if (SelectedCommandSuggestionEvent != null)
                SelectedCommandSuggestionEvent(command);
        }
    }
}