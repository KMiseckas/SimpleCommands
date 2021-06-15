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

using UnityEngine;

namespace SimpleCommands.Runtime.Base
{

    /// <summary>
    /// Parser methods for several different types of instances.
    /// </summary>
    public static class ParserMethods
    {
        /// <summary>
        /// Parse string to return a Vector2.
        /// </summary>
        public static Vector2 ParseVector2(string s)
        {
            var values = s.Split(',');

            return new Vector2(float.Parse(values[0]), float.Parse(values[1]));
        }

        /// <summary>
        /// Parse string to return a Vector3.
        /// </summary>
        public static Vector3 ParseVector3(string s)
        {
            var values = s.Split(',');

            return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        }

        /// <summary>
        /// Parse string to return a Vector4.
        /// </summary>
        public static Vector4 ParseVector4(string s)
        {
            var values = s.Split(',');

            return new Vector4(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
        }

        /// <summary>
        /// Parse string to return a Rect.
        /// </summary>
        public static Rect ParseRect(string s)
        {
            var values = s.Split(',');

            return new Rect(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
        }

        /// <summary>
        /// Parse string where the string represents the runtime ID or name of the gameobject to find and return.
        /// </summary>
        public static GameObject ParseGameObject(string s)
        {
            var data = s.Split(':');

            if (data[0] == "id")
                return ParseGORunTimeID(data[1]);
            else if (data[0] == "n:")
                return ParseGOByName(data[1]);

            return null;
        }

        /// <summary>
        /// Parse string where the string represents the runtime ID of a GameObject to find and return.
        /// </summary>
        private static GameObject ParseGORunTimeID(string s)
        {
            var intID = int.Parse(s);

            var gameObjectsByID = Object.FindObjectsOfType<GameObject>();

            for (var i = 0; i < gameObjectsByID.Length; i++)
                if (gameObjectsByID[i].GetInstanceID() == intID)
                    return gameObjectsByID[i];

            return null;
        }

        /// <summary>
        /// Parse string where the string represents the name of a GameObject to find and return.
        /// </summary>
        private static GameObject ParseGOByName(string s)
        {
            return GameObject.Find(s);
        }
    }
}