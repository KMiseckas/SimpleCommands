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

namespace SimpleCommands
{
    /// <summary>
    /// Interface to be implemented for a class that can return an instance of <see cref="SCCommand"/> that matches a unique key (ID).
    /// The implementation is up to the developer discretion.
    /// </summary>
    public interface ICommandMap
    {
        /// <summary>
        /// Try attempt to get an instance of a command object that matches the unique command key (ID). 
        /// </summary>
        /// <param name="commandKey">The unique command key (ID).</param>
        /// <param name="command">The instance of <see cref="SCCommand"/> object to return. This object contains all relevant information on how the command should be handled.</param>
        /// <returns>True if the command exists and can be retrieved.</returns>
        bool GetCommand(string commandKey, out SCCommand command);
    }
}
