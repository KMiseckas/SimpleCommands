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

            CommandKey = key;
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
            catch
            {
                output = $"Execution for command `{CommandKey}` has failed with unknown reason.";
                return false;
            }

            return true;
        }

        private bool TryFindInstanceByID(TargetIDType idType, string id, out object[] targetObjects)
        {
            targetObjects = null;

            switch(idType)
            {
                case TargetIDType.None:
                    targetObjects = GameObject.FindObjectsOfType(ClassType);
                    break;

                case TargetIDType.Tag:
                    targetObjects = GameObject.FindGameObjectsWithTag(id);
                    targetObjects = targetObjects.Length > 0 ? targetObjects : null;
                    break;

                case TargetIDType.InstanceID:

                    if(!int.TryParse(id, out int intID))
                    {
                        return false;
                    }

                    targetObjects = new object[1];
                    GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();

                    for(int i = 0; i < gameObjects.Length; i++)
                    {
                        if(gameObjects[i].GetInstanceID() == intID)
                        {
                            targetObjects[0] = gameObjects[i];
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
