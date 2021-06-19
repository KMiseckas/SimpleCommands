using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Default implementation of the <see cref="ITypeParsersMap"/> used specifically for the default implementation of the <see cref="ICommandMap"/> in <see cref="CommandMap"/> 
    /// for <see cref="SCCore"/>.<br/><br/>
    /// See <see cref="ITypeParsersMap"/> for explanation.
    /// </summary>
    public class ParsersMap : ITypeParsersMap
    {
        /// <summary>
        /// Static dictionary that contains the types and their respectful parsers of type <see cref="Func{string, object}"/>. These parsers will convert a object in string format
        /// to its object form (if applicable).
        /// </summary>
        private readonly static Dictionary<Type, Func<string, object>> _TypeParsers = new Dictionary<Type, Func<string, object>>();

        /// <summary>
        /// Have statics been initialised already.
        /// </summary>
        private static bool _AreStaticInitialized;

        /// <summary>
        /// Create a new instance of the <see cref="ParsersMap"/>.
        /// </summary>
        public ParsersMap()
        {
            if (!_AreStaticInitialized)
            {
                CreateMap();

                _AreStaticInitialized = true;
            }
        }

        /// <summary>
        /// Create the map of of parsers to their types to parse to. 
        /// </summary>
        protected virtual void CreateMap()
        {
            AddParserFunc(typeof(int), (x) => { return int.Parse(x); });
            AddParserFunc(typeof(float), (x) => { return float.Parse(x); });
            AddParserFunc(typeof(double), (x) => { return double.Parse(x); });
            AddParserFunc(typeof(long), (x) => { return long.Parse(x); });
            AddParserFunc(typeof(byte), (x) => { return byte.Parse(x); });
            AddParserFunc(typeof(uint), (x) => { return uint.Parse(x); });
            AddParserFunc(typeof(bool), (x) => { return bool.Parse(x); });
            AddParserFunc(typeof(string), (x) => { return x; });
            AddParserFunc(typeof(char), (x) => { return char.Parse(x); });
            AddParserFunc(typeof(GameObject), (x) => { return ParserMethods.ParseGameObject(x); });
            AddParserFunc(typeof(Vector2), (x) => { return ParserMethods.ParseVector2(x); });
            AddParserFunc(typeof(Vector3), (x) => { return ParserMethods.ParseVector3(x); });
            AddParserFunc(typeof(Vector4), (x) => { return ParserMethods.ParseVector4(x); });
            AddParserFunc(typeof(Rect), (x) => { return ParserMethods.ParseRect(x); });
        }

        /// <summary>
        /// Add a parser function for a given type.
        /// </summary>
        /// <param name="typeKey">Type the parser being added for is.</param>
        /// <param name="parserFunc">The func that acts as a parser for given parser</param>
        /// <param name="overrideExisting">Should the existing parser for the given type be overriden. False by default.</param>
        protected void AddParserFunc(Type typeKey, Func<string, object> parserFunc, bool overrideExisting = false)
        {
            if (_TypeParsers.ContainsKey(typeKey))
                if (overrideExisting)
                    _TypeParsers.Remove(typeKey);
                else
                    throw new Exception($"Cannot add parser for type `{typeKey.Name}` as it already exists in the class `{GetType().Name}`.");

            _TypeParsers.Add(typeKey, parserFunc);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool GetParser(Type typeToParse, out Func<string, object> parserFunc)
        {
            return _TypeParsers.TryGetValue(typeToParse, out parserFunc);
        }
    }
}
