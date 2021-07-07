// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace SimpleCommands.Runtime.Base
{

    /// <summary>
    /// Parser methods for several different types of instances.
    /// </summary>
    public static class DefaultTypeParsers
    {
        public static string TrimBrackets(string s)
        {
            return s.Trim('(', ')', '[', ']');
        }

        [SCTypeParser]
        public static int ParseInt(string s)
        {
            return int.Parse(s);
        }

        [SCTypeParser]
        public static float ParseFloat(string s)
        {
            return float.Parse(s);
        }

        [SCTypeParser]
        public static double ParseDouble(string s)
        {
            return double.Parse(s);
        }

        [SCTypeParser]
        public static long ParseLong(string s)
        {
            return long.Parse(s);
        }

        [SCTypeParser]
        public static byte ParseByte(string s)
        {
            return byte.Parse(s);
        }

        [SCTypeParser]
        public static uint ParseUInt(string s)
        {
            return uint.Parse(s);
        }

        [SCTypeParser]
        public static bool ParseBool(string s)
        {
            return bool.Parse(s);
        }

        [SCTypeParser]
        public static string ParseString(string s)
        {
            return s;
        }

        [SCTypeParser]
        public static char ParseChar(string s)
        {
            return char.Parse(s);
        }

        /// <summary>
        /// Parse string to return a Vector2.
        /// </summary>
        [SCTypeParser]
        public static Vector2 ParseVector2(string s)
        {
            var values = TrimBrackets(s).Split(',');

            return new Vector2(float.Parse(values[0]), float.Parse(values[1]));
        }

        /// <summary>
        /// Parse string to return a Vector3.
        /// </summary>
        [SCTypeParser]
        public static Vector3 ParseVector3(string s)
        {
            var values = TrimBrackets(s).Split(',');

            return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        }

        /// <summary>
        /// Parse string to return a Vector4.
        /// </summary>
        [SCTypeParser]
        public static Vector4 ParseVector4(string s)
        {
            var values = TrimBrackets(s).Split(',');

            return new Vector4(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
        }

        /// <summary>
        /// Parse string to return a Rect.
        /// </summary>
        [SCTypeParser]
        public static Rect ParseRect(string s)
        {
            var values = TrimBrackets(s).Split(',');

            return new Rect(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
        }

        /// <summary>
        /// Parse string where the string represents the runtime ID or name of the gameobject to find and return.
        /// </summary>
        [SCTypeParser]
        public static GameObject ParseGameObject(string s)
        {
            var data = TrimBrackets(s).Split(':');

            if (data[0] == "id")
                return ParseGORunTimeID(data[1]);
            else if (data[0] == "n:")
                return ParseGOByName(data[1]);
            else if (data[0] == "t:")
                return ParseGOByTag(data[1]);

            return null;
        }

        /// <summary>
        /// Parse string where the string represents the runtime ID or name of the gameobject to find and return.
        /// </summary>
        [SCTypeParser]
        public static GameObject[] ParseGameObjects(string s)
        {
            s = TrimBrackets(s).Trim();

            if (s.StartsWith("{") && s.EndsWith("}"))
            {
                s = s.Remove(0);
                s = s.Remove(s.Length - 1);

                List<GameObject> gameObjects = new List<GameObject>();

                var arrayElements = s.Split(',');

                for (int i = 0; i < arrayElements.Length; i++)
                {
                    GameObject[] gameObject = null;

                    var data = arrayElements[i].Split(':');

                    if (data[0] == "id")
                        gameObject = new GameObject[] { ParseGORunTimeID(data[1]) };
                    else if (data[0] == "n:")
                        gameObject = new GameObject[] { ParseGOByName(data[1]) };
                    else if (data[0] == "t:")
                        gameObject = new GameObject[] { ParseGOByTag(data[1]) };
                    else if (data[0] == "t-all:")
                        gameObject = ParseGOsByTag(data[1]);

                    for (int j = 0; j < gameObject.Length; j++)
                    {
                        if (gameObject[j] != null)
                        {
                            gameObjects.Add(gameObject[j]);
                        }
                        else
                        {
                            SCBase.OutConsole($"Could not find gameobject for element [{data[0]}:{data[1]}]", OutputType.WARNING);
                        }
                    }
                }

                return gameObjects.ToArray();
            }
            else
            {
                var data = s.Split(':');

                if (data[0] == "t-all:")
                    return ParseGOsByTag(data[1]);
            }

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

        /// <summary>
        /// Parse string where the string represents the tag of a GameObject to find and return.
        /// </summary>
        private static GameObject ParseGOByTag(string s)
        {
            return GameObject.FindGameObjectWithTag(s);
        }

        /// <summary>
        /// Parse string where the string represents the tag of a GameObjects to find and return.
        /// </summary>
        private static GameObject[] ParseGOsByTag(string s)
        {
            return GameObject.FindGameObjectsWithTag(s);
        }
    }
}