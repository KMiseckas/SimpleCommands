using System;
using System.Collections.Generic;
using SimpleCommands;
using SimpleCommands.Base.Interfaces;
using UnityEngine;

namespace SimpleCommands.Implementations
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
    public class CommandTargetParserMap : ITargetParser
    {
        /// <summary>
        /// Instance of the disctionary that maps the id type to its parser Func object.
        /// </summary>
        private readonly static Dictionary<string, Func<SCCommand, string, object[]>> TARGET_PARSER_MAP = new Dictionary<string, Func<SCCommand, string, object[]>>();

        /// <summary>
        /// Create a new instance of <see cref="CommandTargetParserMap"/>.
        /// </summary>
        protected internal CommandTargetParserMap()
        {
            AddTargetParsers();
        }

        /// <summary>
        /// Add all the desired target parsers to the map.
        /// </summary>
        protected virtual void AddTargetParsers()
        {
            AddTargetParser("default", GetDefaultTargets);
            AddTargetParser("tag", GetTargetsByTag);
            AddTargetParser("t", GetTargetsByTag);
            AddTargetParser("name", GetTargetsByName);
            AddTargetParser("n", GetTargetsByName);
            AddTargetParser("id", GetTargetsByInstanceID);
        }

        /// <summary>
        /// Add an instance of a target parser to the map.
        /// </summary>
        /// <param name="idType">ID type of the parser. Acts as the key in the map.</param>
        /// <param name="parserFunc">Parser value. Function which will find all objects when provided the id.</param>
        protected void AddTargetParser(string idType, Func<SCCommand, string, object[]> parserFunc)
        {
            TARGET_PARSER_MAP.Add(idType, parserFunc);
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

            return TARGET_PARSER_MAP.TryGetValue(idType, out targetObjectParser);
        }

        /// <summary>
        /// Function / Parser that retrieves all object instances in scene that can issue the command.
        /// </summary>
        /// <param name="commandData">The command that the user is trying to execute.</param>
        /// <param name="targetInput">The string within the target input as entered by user.</param>
        /// <returns>List of objects that can have the command executed from.</returns>
        internal virtual object[] GetDefaultTargets(SCCommand commandData, string targetInput)
        {
            return GameObject.FindObjectsOfType(commandData.ClassType);
        }

        /// <summary>
        /// Function / Parser that retrieves all object instances in scene with the specific tag and that can issue the command.
        /// </summary>
        /// <param name="commandData">The command that the user is trying to execute.</param>
        /// <param name="targetInput">The string within the target input as entered by user.</param>
        /// <returns>List of objects that can have the command executed from.</returns>
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

        /// <summary>
        /// Function / Parser that retrieves all object instances in scene with the specific runtime instance ID and that can issue the command.
        /// </summary>
        /// <param name="commandData">The command that the user is trying to execute.</param>
        /// <param name="targetInput">The string within the target input as entered by user.</param>
        /// <returns>List of objects that can have the command executed from.</returns>
        internal virtual object[] GetTargetsByInstanceID(SCCommand commandData, string targetInput)
        {
            if (!int.TryParse(targetInput, out int intID))
            {
                return null;
            }

            GameObject[] gameObjectsByID = GameObject.FindObjectsOfType<GameObject>();

            for (int i = 0; i < gameObjectsByID.Length; i++)
            {
                if (gameObjectsByID[i].GetInstanceID() == intID)
                {
                    return new object[] { gameObjectsByID[i] };
                }
            }

            return null;
        }

        /// <summary>
        /// Function / Parser that retrieves all object instances in scene with the specific name and that can issue the command.
        /// </summary>
        /// <param name="commandData">The command that the user is trying to execute.</param>
        /// <param name="targetInput">The string within the target input as entered by user.</param>
        /// <returns>List of objects that can have the command executed from.</returns>
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