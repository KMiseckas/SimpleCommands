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