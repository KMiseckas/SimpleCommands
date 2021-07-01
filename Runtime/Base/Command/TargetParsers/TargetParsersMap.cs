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
using System.Reflection;
using UnityEngine;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Default implementation of the <see cref="ITargetParser"/> interface. Provides access to common tag types and their parsers that might be used within any Unity game. 
    /// <br/><br/>
    /// This includes:<br/>
    /// - <b>default</b>: Finds all objects that can fire off the command <see cref="GetDefaultTargets(SCCommand, string)"/>.<br/>
    /// - <b>tag</b>: Finds all objects that contain the specific unity tag <see cref="GetTargetsByTag(SCCommand, string)"/>.<br/>
    /// - <b>t</b>: Same as tag, but abbreviated.<br/>
    /// - <b>name</b>: Finds all objects that contain the specific name <see cref="GetTargetsByName(SCCommand, string)"/>.<br/>
    /// - <b>n</b>: Same as name but abbreviated.<br/>
    /// - <b>id</b>: Finds all objects based on the runtime instance ID <see cref="GetTargetsByInstanceID(SCCommand, string)"/>.<br/>
    /// </summary>
    public class TargetParsersMap
    {
        /// <summary>
        /// Instance of the disctionary that maps the id type to its parser Func object.
        /// </summary>
        private readonly static Dictionary<string, TargetParserInfo> TARGET_PARSER_MAP = new Dictionary<string, TargetParserInfo>();

        /// <summary>
        /// Have statics been initialised already.
        /// </summary>
        private static bool _AreStaticInitialized;

        /// <summary>
        /// Create a new instance of <see cref="TargetParsersMap"/>.
        /// </summary>
        protected internal TargetParsersMap()
        {
            if (!_AreStaticInitialized)
            {
                CreateMap();

                _AreStaticInitialized = true;
            }
        }

        protected virtual void CreateMap()
        {
            var parserMethodInfo = ReflectionUtils.FindAttributeMethodInfo<SCTargetParserAttribute>(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

            //For every method information stored.
            for (var i = 0; i < parserMethodInfo.Length; i++)
            {
                var method = parserMethodInfo[i].MethodInfo;
                var attribute = parserMethodInfo[i].Attribute;

                AddTargetParser(attribute.Key, (x, y) => { return (object[])method.Invoke(null, new object[] { x, y }); }, attribute.Priority);
            }
        }

        /// <summary>
        /// Add an instance of a target parser to the map.
        /// </summary>
        /// <param name="key">ID type of the parser. Acts as the key in the map.</param>
        /// <param name="parserFunc">Parser value. Function which will find all objects when provided the id.</param>
        protected void AddTargetParser(string key, Func<SCCommand, string, object[]> parserFunc, int priority)
        {
            if (TARGET_PARSER_MAP.ContainsKey(key))
            {
                TARGET_PARSER_MAP.TryGetValue(key, out TargetParserInfo info);

                if (priority >= info.Priority)
                {
                    TARGET_PARSER_MAP.Remove(key);

                    SCBase.OutConsole($"Overriding parser for target ID of [{key}] with priority level of [{priority}].", OutputType.INFO);
                }
            }

            TARGET_PARSER_MAP.Add(key, new TargetParserInfo(parserFunc, priority));
        }

        /// <summary>
        /// Try and get an instance of the parser if it exists for the given id type.
        /// </summary>
        /// <param name="idType">ID type.</param>
        /// <param name="targetObjectParser">Object that is returned.</param>
        /// <returns>True if the parser exists and has been found.</returns>
        public bool TryGetParser(string idType, out Func<SCCommand, string, object[]> targetObjectParser)
        {
            if (idType == null)
                idType = "default";

            targetObjectParser = null;

            if (!TARGET_PARSER_MAP.TryGetValue(idType, out TargetParserInfo targetParserInfo)) return false;

            targetObjectParser = targetParserInfo.ParserFunc;

            return true;
        }

        public struct TargetParserInfo
        {
            public readonly Func<SCCommand, string, object[]> ParserFunc;

            public readonly int Priority;

            public TargetParserInfo(Func<SCCommand, string, object[]> func, int priority)
            {
                ParserFunc = func;
                Priority = priority;
            }
        }
    }
}