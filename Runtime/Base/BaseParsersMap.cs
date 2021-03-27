using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace SimpleCommands
{
    public class BaseParsersMap : IParsersMap
    {
        private readonly static Dictionary<Type, Func<string, object>> _TypeParsers = new Dictionary<Type, Func<string, object>>();

        private static bool _AreStaticInitialized;

        public BaseParsersMap()
        {
            if(!_AreStaticInitialized)
            {
                CreateMap();

                _AreStaticInitialized = true;
            }
        }

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
        }

        protected void AddParserFunc(Type typeKey, Func<string, object> parserFunc, bool overrideExisting = false)
        {
            if(_TypeParsers.ContainsKey(typeKey))
            {
                if(overrideExisting)
                {
                    _TypeParsers.Remove(typeKey);
                }
                else
                {
                    throw new Exception($"Cannot add parser for type `{typeKey.Name}` as it already exists in the class `{this.GetType().Name}`.");
                }
            }

            _TypeParsers.Add(typeKey, parserFunc);
        }

        public bool GetParser(Type typeToParse, out Func<string, object> parserFunc)
        {
            return _TypeParsers.TryGetValue(typeToParse, out parserFunc);
        }
    }
}
