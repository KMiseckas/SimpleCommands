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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleCommands
{
    /// <summary>
    /// Object that contains the information neccessery to execute the command intent in the game.
    /// </summary>
    public class SCCommand
    {
        /// <summary>
        /// Unique key (ID) that will trigger the intent execution.
        /// </summary>
        internal readonly string CommandKey;

        /// <summary>
        /// The description of the command intent.
        /// </summary>
        internal readonly string CommandDesc;

        /// <summary>
        /// The type of class that this command invokes a method on.
        /// </summary>
        internal readonly Type ClassType;

        /// <summary>
        /// An array of parameters and their info that the method takes when invoked due to command execution.
        /// </summary>
        internal readonly ParamInfo[] ParamInfo;

        /// <summary>
        /// The info of the method that this command will invoke upon execution.
        /// </summary>
        internal readonly MethodInfo Method; 

        /// <summary>
        /// Create a new instance of <see cref="SCCommand"/>.
        /// </summary>
        /// <param name="key">Unique string key of the command.</param>
        /// <param name="description">Description of the command intent.</param>
        /// <param name="classType">The type of class this command will invoke a method on.</param>
        /// <param name="paramInfo">The parameters and their information on the method this command will invoke.</param>
        /// <param name="method">Information on the method that this command will invoke.</param>
        internal SCCommand(string key, string description, Type classType, ParamInfo[] paramInfo, MethodInfo method)
        {
            Assert.IsNotNull(key);
            Assert.IsNotNull(paramInfo);
            Assert.IsNotNull(classType);
            Assert.IsNotNull(method);

            CommandKey = key.ToLower();
            CommandDesc = description;
            ClassType = classType;
            ParamInfo = paramInfo;
            Method = method;
        }

        /// <summary>
        /// Try attempt to execute the commands intent onto the game using the parameters and information provided.
        /// </summary>
        /// <param name="paramVals">The values as received from the command input string.</param>
        /// <param name="output">The string message output received from the execution proccess for the console output.</param>
        /// <param name="targetInfo">The target info to tell on what to execute the commands intent on.</param>
        /// <returns>True if execution succeeded.</returns>
        internal bool TryExecute(string[] paramVals, out string output, TargetInfo targetInfo = default)
        {
            Assert.IsNotNull(paramVals);

            output = "";

            object[] parsedParams = null;

            //If parameters do exist, try parse their string types into objects.
            if(ParamInfo.Length > 0)
            {
                if(!TryParseParams(paramVals, out parsedParams, out output))
                {
                    return false;
                }
            }

            object[] targetInstances = null;
            bool instanceFound = false;

            if(!Method.IsStatic)
            {
                instanceFound = TryFindInstanceByID(targetInfo.IDType, targetInfo.ID, out targetInstances);
            }

            try
            {
                if(instanceFound)
                {
                    for(int i = 0; i < targetInstances.Length; i++)
                    {
                        Method.Invoke(targetInstances[i], parsedParams);
                    }
                }
                else
                {
                    Method.Invoke(null, parsedParams);
                }
            }
            catch(Exception exception)
            {
                output = $"Execution for command `{CommandKey}` has failed: {exception.Message}";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempt to find an instance or instances on which to invoke the intent of the command on.
        /// </summary>
        /// <param name="idType">The type of target ID that has been given for this command.</param>
        /// <param name="id">String ID of the object to execute the intent on. Different targets will require different IDs.</param>
        /// <param name="targetObjects">Output array of objects to execute the commands intent on.</param>
        /// <returns>True if instance found.</returns>
        private bool TryFindInstanceByID(TargetIDType idType, string id, out object[] targetObjects)
        {
            //TODO add failure output strings.
            targetObjects = null;

            switch(idType)
            {
                case TargetIDType.None:
                    targetObjects = GameObject.FindObjectsOfType(ClassType);
                    break;

                case TargetIDType.Tag:
                    GameObject[] gameObjectsByTag = null;

                    try
                    {
                        gameObjectsByTag = GameObject.FindGameObjectsWithTag(id);
                    }
                    catch
                    {
                        return false;
                    }

                    gameObjectsByTag = gameObjectsByTag.Length > 0 ? gameObjectsByTag : null;

                    if(gameObjectsByTag == null)
                        return false;

                    List<object> components = new List<object>();

                    for(int i = 0; i < gameObjectsByTag.Length; i++)
                    {
                        GameObject gameObject = gameObjectsByTag[i];

                        object component = gameObject.GetComponent(ClassType);

                        if(component != null)
                        {
                            components.Add(component);
                        }
                    }

                    if(components.Count > 0)
                    {
                        targetObjects = components.ToArray();
                    }

                    break;

                case TargetIDType.InstanceID:

                    if(!int.TryParse(id, out int intID))
                    {
                        return false;
                    }

                    targetObjects = new object[1];
                    GameObject[] gameObjectsByID = GameObject.FindObjectsOfType<GameObject>();

                    for(int i = 0; i < gameObjectsByID.Length; i++)
                    {
                        if(gameObjectsByID[i].GetInstanceID() == intID)
                        {
                            targetObjects[0] = gameObjectsByID[i];
                        }
                    }

                    break;
            }

            return true;
        }

        /// <summary>
        /// Try parse the string parameters of the command input string into objects.
        /// </summary>
        /// <param name="paramVals">The parameter string values.</param>
        /// <param name="parsedParams">The array of parameters as parsed from string values.</param>
        /// <param name="failOutput">Output message for the fail, if a fail happens only.</param>
        /// <returns></returns>
        private bool TryParseParams(string[] paramVals, out object[] parsedParams, out string failOutput)
        {
            failOutput = "";

            int paramValCount = paramVals.Length;

            parsedParams = new object[ParamInfo.Length];

            for(int i = 0; i < ParamInfo.Length; i++)
            {
                if(paramValCount > i)
                {
                    try
                    {
                        parsedParams[i] = ParamInfo[i].ParserFunc.Invoke(paramVals[i]);
                    }
                    catch
                    {
                        parsedParams = null;
                        failOutput = $"Could not parse `{paramVals[i]}` as type `{ParamInfo[i].Type.Name}` when executing command.";
                        return false;
                    }

                    continue;
                }

                if(!ParamInfo[i].IsOptional)
                {
                    parsedParams = null;
                    failOutput = $"Parameter at position `{i}` for type `{ParamInfo[i].Type.Name}` must be specified.";
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Data class for storing a single parameter information.
    /// </summary>
    public struct ParamInfo
    {
        /// <summary>
        /// Type of object the parameter is.
        /// </summary>
        internal readonly Type Type;

        /// <summary>
        /// The parser that will parse the string value to a concrete type for this parameter.
        /// </summary>
        internal readonly Func<string, object> ParserFunc;

        /// <summary>
        /// Is this an optional parameter, and can be it skipped if not provided within the command input string.
        /// </summary>
        internal readonly bool IsOptional;

        /// <summary>
        /// Create a new instance of <see cref="ParamInfo"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parserFunc"></param>
        /// <param name="isOptionalParam"></param>
        public ParamInfo(Type type, Func<string, object> parserFunc, bool isOptionalParam = false)
        {
            Type = type;
            ParserFunc = parserFunc;
            IsOptional = isOptionalParam;
        }
    }

    /// <summary>
    /// The information for the target of the command intent execution.
    /// </summary>
    public struct TargetInfo
    {
        /// <summary>
        /// Type of target.
        /// </summary>
        internal TargetIDType IDType;

        /// <summary>
        /// Target type ID by which to identify the specific target. Different target types may have different ID formats.
        /// </summary>
        internal string ID;
    }

    /// <summary>
    /// Target types supported by default.<br/><br/>
    /// - Tag: Unity tag system, where ID is the name of the tag.<br/>
    /// - InstanceID: Unity object unique ID, where ID is the unique numerical ID of the object at runtime.<br/>
    /// - None: Will execute the intent on all found instances.<br/><br/>
    /// 
    /// Static methods will not reach this functionality and so do not require a target ID assigning to them.
    /// </summary>
    public enum TargetIDType
    {
        None,
        Tag,
        InstanceID
    }
}
