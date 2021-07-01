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
using System.Globalization;
using System.Reflection;
using UnityEngine;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Default implementation of the <see cref="ITypeParsersMap"/> used specifically for the default implementation of the <see cref="ICommandMap"/> in <see cref="CommandMap"/> 
    /// for <see cref="SCCore"/>.<br/><br/>
    /// See <see cref="ITypeParsersMap"/> for explanation.
    /// </summary>
    public class TypeParsersMap
    {
        /// <summary>
        /// Static dictionary that contains the types and their respectful parsers of type <see cref="Func{string, object}"/>. These parsers will convert a object in string format
        /// to its object form (if applicable).
        /// </summary>
        private readonly static Dictionary<Type, ParserInfo> _TypeParsers = new Dictionary<Type, ParserInfo>();

        /// <summary>
        /// Have statics been initialised already.
        /// </summary>
        private static bool _AreStaticInitialized;

        /// <summary>
        /// Create a new instance of the <see cref="TypeParsersMap"/>.
        /// </summary>
        protected internal TypeParsersMap()
        {
            if (!_AreStaticInitialized)
            {
                CreateMap();

                _AreStaticInitialized = true;
            }
        }

        protected virtual void CreateMap()
        {
            var parserMethodInfo = MethodScanner.FindAttributeMethodInfo<SCTypeParserAttribute>(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

            //For every method information stored.
            for (var i = 0; i < parserMethodInfo.Length; i++)
            {
                var commandMethod = parserMethodInfo[i].MethodInfo;
                var methodParams = commandMethod.GetParameters();
                var attribute = parserMethodInfo[i].Attribute;
                var methodParamCount = methodParams.Length;
                var cmdParamInfo = new ParamInfo[methodParamCount];

                AddParserFunc(commandMethod.ReturnType, (x) => { return commandMethod.Invoke(null, new object[] { x }); }, attribute.Priority);
            }
        }


        /// <summary>
        /// Add a parser function for a given type.
        /// </summary>
        /// <param name="typeKey">Type the parser being added for is.</param>
        /// <param name="parserFunc">The func that acts as a parser for given parser</param>
        /// <param name="overrideExisting">Should the existing parser for the given type be overriden. False by default.</param>
        protected virtual void AddParserFunc(Type typeKey, Func<string, object> parserFunc, int priority)
        {
            if (_TypeParsers.ContainsKey(typeKey))
            {
                _TypeParsers.TryGetValue(typeKey, out ParserInfo info);

                if (priority >= info.Priority)
                {
                    _TypeParsers.Remove(typeKey);

                    SCBase.OutConsole($"Overriding parser for type [{typeKey.Name}] with priority level of [{priority}].", OutputType.INFO);
                }
            }

            _TypeParsers.Add(typeKey, new ParserInfo(parserFunc, priority));
        }

        public virtual bool GetParser(Type typeToParse, out Func<string, object> parserFunc)
        {
            parserFunc = null;

            if (!_TypeParsers.TryGetValue(typeToParse, out ParserInfo info)) return false;

            parserFunc = info.ParserFunc;

            return true;
        }

        public struct ParserInfo
        {
            public readonly Func<string, object> ParserFunc;

            public readonly int Priority;

            public ParserInfo(Func<string, object> func, int priority)
            {
                ParserFunc = func;
                Priority = priority;
            }
        }
    }
}
