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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleCommands
{
    /// <summary>
    /// Interface to be implemented by a class which can parse a command input string into an instance of <see cref="CommandInputInfo"/>.
    /// </summary>
    public interface ICommandInputParser
    {
        /// <summary>
        /// Try attempt to parse a command input string. If successful, return an instance of <see cref="CommandInputInfo"/> which contains the information gathered from the parsed 
        /// command string.
        /// </summary>
        /// <param name="commandInput">The command input string.</param>
        /// <param name="commandInputInfo">On success, the instance populated with parsed information.</param>
        /// <returns>True if parsing did not fail.</returns>
        bool TryParseCommandInput(string commandInput, out CommandInputInfo commandInputInfo);
    }
}
