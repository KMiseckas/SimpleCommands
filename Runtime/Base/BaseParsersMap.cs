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
