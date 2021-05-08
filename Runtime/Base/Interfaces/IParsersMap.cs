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
using System.Collections.Generic;

namespace SimpleCommands
{
    /// <summary>
    /// Interface to be implemented by a class that maps parsers to their Types to parse. The implementation should be able to get a parser for a specific object Type. The class should be able to retrieve a instance
    /// of <see cref="Func{string, object}<"/> which will act as the parser, where `string` is the specific object that needs parsing, but in string form,
    /// and where `object` is the object that will be returned on a successful parse.<br/><br/>
    /// 
    /// <see cref="IParsersMap"/> is not inherently used within the `SimpleCommands` base project, but is one of the main features for the default implementation of command
    /// retrieval in the implementation of the <see cref="ICommandMap"/>. However, this is not neccessery if a custom solution is used for parsing and retrieving commands
    /// based on input.
    /// </summary>
    public interface IParsersMap
    {

        /// <summary>
        /// Try and get the parser for the specified <see cref="Type"/>. The parser is represented in the form of <see cref="Func{string, object}"/>
        /// where `string` is the specific object that needs parsing, but in string form,and where `object` is the object that will be returned on a successful parse.
        /// </summary>
        /// <param name="typeToParse">Type trying to parse.</param>
        /// <param name="paramParser">Instance of parser to return.</param>
        /// <returns>True if the parser has been found.</returns>
        bool GetParser(Type typeToParse, out Func<string, object> paramParser);
    }
}
