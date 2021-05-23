using System;
using System.Collections.Generic;
using SimpleCommands;
using UnityEngine;

namespace SimpleCommands
{
    public class CommandTargetParserMap : ITargetParser
    {
        private readonly static Dictionary<string, Func<SCCommand, string, object[]>> TARGET_PARSER_MAP = new Dictionary<string, Func<SCCommand, string, object[]>>();

        protected internal CommandTargetParserMap()
        {
            AddTargetParsers();
        }

        protected virtual void AddTargetParsers()
        {
            AddTargetParser("default", GetDefaultTargets);
            AddTargetParser("tag", GetTargetsByTag);
            AddTargetParser("t", GetTargetsByTag);
            AddTargetParser("name", GetTargetsByName);
            AddTargetParser("n", GetTargetsByName);
            AddTargetParser("id", GetTargetsByInstanceID);
        }

        protected void AddTargetParser(string idType, Func<SCCommand, string, object[]> parserFunc)
        {
            TARGET_PARSER_MAP.Add(idType, parserFunc);
        }

        public bool TryGetParser(string idType, out Func<SCCommand, string, object[]> targetObjectParser)
        {
            if (idType == null)
                idType = "default";

            return TARGET_PARSER_MAP.TryGetValue(idType, out targetObjectParser);
        }

        internal virtual object[] GetDefaultTargets(SCCommand commandData, string targetInput)
        {
            return GameObject.FindObjectsOfType(commandData.ClassType);
        }

        internal virtual object[] GetTargetsByTag(SCCommand commandData, string targetInput)
        {
            GameObject[] gameObjectsByTag = null;

            try
            {
                gameObjectsByTag = GameObject.FindGameObjectsWithTag(targetInput);
            }
            catch
            {
                return null;
            }

            gameObjectsByTag = gameObjectsByTag.Length > 0 ? gameObjectsByTag : null;

            if (gameObjectsByTag == null)
                return null;

            List<object> components = new List<object>();

            for (int i = 0; i < gameObjectsByTag.Length; i++)
            {
                GameObject gameObject = gameObjectsByTag[i];

                object component = gameObject.GetComponent(commandData.ClassType);

                if (component != null)
                {
                    components.Add(component);
                }
            }

            if (components.Count > 0)
            {
                return components.ToArray();
            }

            return null;
        }

        internal virtual object[] GetTargetsByInstanceID(SCCommand commandData, string targetInput)
        {
            if (!int.TryParse(targetInput, out int intID))
            {
                return null;
            }

            object targetObject = null;

            GameObject[] gameObjectsByID = GameObject.FindObjectsOfType<GameObject>();

            for (int i = 0; i < gameObjectsByID.Length; i++)
            {
                if (gameObjectsByID[i].GetInstanceID() == intID)
                {
                    targetObject = gameObjectsByID[i];
                }
            }

            return targetObject == null ? null : new object[]{targetObject };
        }

        internal virtual object[] GetTargetsByName(SCCommand commandData, string targetInput)
        {
            GameObject gameObject = GameObject.Find(targetInput);

            object component = gameObject.GetComponent(commandData.ClassType);

            if (component != null)
            {
                return new object[] { component };
            }

            return null;
        }
    }
}