// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Assertions;

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
            var parserMethodInfo = ReflectionUtils.FindAttributeMethodInfo<SCTypeParserAttribute>(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

            //For every method information stored.
            for (var i = 0; i < parserMethodInfo.Length; i++)
            {
                var parserMethod = parserMethodInfo[i].MethodInfo;
                var methodParams = parserMethod.GetParameters();
                var attribute = parserMethodInfo[i].Attribute;

                Assert.IsTrue(methodParams.Length == 1, $"Method of [{parserMethod.Name}] in [{parserMethod.DeclaringType}] with attribute [SCTypeParser] must only contain a single parameter");
                Assert.IsTrue(methodParams[0].ParameterType.Equals(typeof(string)), $"Method of [{parserMethod.Name}] in [{parserMethod.DeclaringType}] with attribute [SCTypeParser] must only contain a single parameter of type `string`");
                Assert.IsTrue(!parserMethod.ReturnType.Equals(typeof(void)), $"Method of [{parserMethod.Name}] in [{parserMethod.DeclaringType}] with attribute [SCTypeParser] must return the `Type` of object that it is supposed to parse");

                AddParserFunc(parserMethod.ReturnType, (x) => { return parserMethod.Invoke(null, new object[] { x }); }, attribute.Priority);
            }
        }


        /// <summary>
        /// Add a parser function for a given type.
        /// </summary>
        /// <param name="typeKey">Type the parser being added for is.</param>
        /// <param name="parserFunc">The func that acts as a parser for given parser</param>
        /// <param name="overrideExisting">Should the existing parser for the given type be overriden. False by default.</param>
        protected void AddParserFunc(Type typeKey, Func<string, object> parserFunc, int priority)
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

        public bool GetParser(Type typeToParse, out Func<string, object> parserFunc)
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
