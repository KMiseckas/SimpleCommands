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

namespace SimpleCommands.Attributes
{
    /// <summary>
    /// Attribute that defines any method as a command that is compatible with this `SimpleCommands` project.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SCCommandAttribute : Attribute
    {
        /// <summary>
        /// The unique key for the command to be issued.
        /// </summary>
        public readonly string CommandKey;

        /// <summary>
        /// The description of the command intent.
        /// </summary>
        public readonly string CommandDescription;

        /// <summary>
        /// Create a new instance of the <see cref="SCCommandAttribute"/>.
        /// </summary>
        /// <param name="commandKey">The unique key which will execute the command.</param>
        /// <param name="commandDescription">The description of the command intent.</param>
        public SCCommandAttribute(string commandKey, string commandDescription = "")
        {
            CommandKey = commandKey.ToLower();
            CommandDescription = commandDescription;
        }
    }
}
