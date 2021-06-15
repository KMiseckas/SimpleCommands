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

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Interface to be implemented by a class that maps parsers of different target IDs types.<br/><br/>
    /// 
    ///  Parsers should be of type Func<SCCommand, string, object[]>, where the func will output the list of objects that the specific target string is trying to target for a command.
    ///
    /// </summary>
    public interface ITargetParser
    {
        /// <summary>
        /// Try to get a parser for the id type that is provided.
        /// </summary>
        /// <param name="idType">The type of ID that is being provided to the parser.</param>
        /// <param name="targetObjectParser">Object to output.</param>
        /// <returns>If succesfull, an instance of a parser for the target ID type provided.</returns>
        bool TryGetParser(string idType, out Func<SCCommand, string, object[]> targetObjectParser);
    }
}
