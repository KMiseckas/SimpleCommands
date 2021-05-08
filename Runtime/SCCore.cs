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

namespace SimpleCommands
{
    /// <summary>
    /// Extension of the base class for simple commands. Implements the abstract methods required for `SimpleCommands` functionality to work. 
    /// - Creates an implementation of a <see cref="ICommandMap"/>, by default this is an instance of <see cref="CommandMap"/>.<br/>
    /// - Creates an implementation of a <see cref="ICommandInputParser"/>, by default this is an instance of <see cref="CommandInputParser"/>.<br/>
    /// - Creates an additional implementation of <see cref="IParsersMap"/> which by default is an instance of <see cref="ParsersMap"/>.
    /// <br/><br/>
    /// The method of <see cref="CreateParserMap"/> can be overriden in an extended class to return a custom <see cref="IParsersMap"/> implementation. It is recommended to 
    /// extend the class of <see cref="ParsersMap"/> and add your own parsers for any object type you may want to be able to parse through a command execution (for example, 
    /// you may want to add a parser for a Vector4"/>.
    /// <br/><br/>
    /// For more info see <see cref="ParsersMap"/>.
    /// <br/><br/>
    /// </summary>
    public class SCCore : SCBase
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override ICommandMap CreateCommandMap()
        {
            return new CommandMap(CreateParserMap());
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override ICommandInputParser CreateCommandInputParser()
        {
            return new CommandInputParser();
        }

        /// <summary>
        /// Create a new implementation instance of <see cref="IParsersMap"/>.
        /// </summary>
        /// <returns>New instances <see cref="IParsersMap"/> implementation.</returns>
        protected virtual IParsersMap CreateParserMap()
        {
            return new ParsersMap();
        }
    }
}
