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
    public class SCCommand
    {
        internal readonly string CommandKey;

        internal readonly string CommandDesc;

        internal readonly Type ClassType;

        internal readonly ParamInfo[] ParamInfo;

        internal readonly MethodInfo Method; 

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

        internal bool TryExecute(string[] paramVals, out string output, TargetInfo targetInfo = default)
        {
            Assert.IsNotNull(paramVals);

            output = "";

            object[] parsedParams = null;

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

    public struct ParamInfo
    {
        internal readonly Type Type;
        internal readonly Func<string, object> ParserFunc;
        internal readonly bool IsOptional;

        public ParamInfo(Type type, Func<string, object> parserFunc, bool isOptionalParam = false)
        {
            Type = type;
            ParserFunc = parserFunc;
            IsOptional = isOptionalParam;
        }
    }

    public struct TargetInfo
    {
        internal TargetIDType IDType;
        internal string ID;
    }

    public enum TargetIDType
    {
        None,
        Tag,
        InstanceID
    }
}
