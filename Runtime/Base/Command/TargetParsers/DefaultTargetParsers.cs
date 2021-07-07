// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCommands.Runtime.Base
{
    public static class DefaultTargetParsers
    {
        /// <summary>
        /// Function / Parser that retrieves all object instances in scene that can issue the command.
        /// </summary>
        /// <param name="commandData">The command that the user is trying to execute.</param>
        /// <param name="targetInput">The string within the target input as entered by user.</param>
        /// <returns>List of objects that can have the command executed from.</returns>
        [SCTargetParser("default")]
        private static object[] GetDefaultTargets(SCCommand commandData, string targetInput)
        {
            return Object.FindObjectsOfType(commandData.ClassType);
        }

        /// <summary>
        /// Function / Parser that retrieves all object instances in scene with the specific tag and that can issue the command.
        /// </summary>
        /// <param name="commandData">The command that the user is trying to execute.</param>
        /// <param name="targetInput">The string within the target input as entered by user.</param>
        /// <returns>List of objects that can have the command executed from.</returns>
        [SCTargetParser("tag")]
        [SCTargetParser("t")]
        private static object[] GetTargetsByTag(SCCommand commandData, string targetInput)
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

            var components = new List<object>();

            for (var i = 0; i < gameObjectsByTag.Length; i++)
            {
                var gameObject = gameObjectsByTag[i];

                object component = gameObject.GetComponent(commandData.ClassType);

                if (component != null)
                    components.Add(component);
            }

            if (components.Count > 0)
                return components.ToArray();

            return null;
        }

        /// <summary>
        /// Function / Parser that retrieves all object instances in scene with the specific runtime instance ID and that can issue the command.
        /// </summary>
        /// <param name="commandData">The command that the user is trying to execute.</param>
        /// <param name="targetInput">The string within the target input as entered by user.</param>
        /// <returns>List of objects that can have the command executed from.</returns>
        [SCTargetParser("id")]
        private static object[] GetTargetsByInstanceID(SCCommand commandData, string targetInput)
        {
            if (!int.TryParse(targetInput, out var intID))
                return null;

            var gameObjectsByID = Object.FindObjectsOfType<GameObject>();

            for (var i = 0; i < gameObjectsByID.Length; i++)
                if (gameObjectsByID[i].GetInstanceID() == intID)
                    return new object[] { gameObjectsByID[i] };

            return null;
        }

        /// <summary>
        /// Function / Parser that retrieves all object instances in scene with the specific name and that can issue the command.
        /// </summary>
        /// <param name="commandData">The command that the user is trying to execute.</param>
        /// <param name="targetInput">The string within the target input as entered by user.</param>
        /// <returns>List of objects that can have the command executed from.</returns>
        [SCTargetParser("n")]
        [SCTargetParser("name")]
        private static object[] GetTargetsByName(SCCommand commandData, string targetInput)
        {
            var gameObject = GameObject.Find(targetInput);

            object component = gameObject.GetComponent(commandData.ClassType);

            if (component != null)
                return new object[] { component };

            return null;
        }
    }
}