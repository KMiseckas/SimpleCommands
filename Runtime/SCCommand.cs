using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SimpleCommands
{
    public class SCCommand
    {
        internal readonly string CommandKey;

        internal readonly string CommandDesc;

        internal readonly ParamInfo[] ParamInfo;

        internal readonly MethodInfo Method; 

        internal SCCommand(string key, string description, ParamInfo[] paramInfo, MethodInfo method)
        {
            CommandKey = key;
            CommandDesc = description;
            ParamInfo = paramInfo;
            Method = method;
        }

        public bool Execute(string[] paramVals, out string output)
        {
            output = $"Executed `{CommandKey}` with params: `{string.Join(", ", paramVals)}`";

            object[] parsedParams = null;

            if(ParamInfo.Length > 0)
            {
                if(!ParseParams(paramVals, out parsedParams, out output))
                {
                    return false;
                }
            }

            try
            {
                Method.Invoke(null, parsedParams);
            }
            catch
            {
                output = $"Execution for command `{CommandKey}` has failed";
                return false;
            }

            return true;
        }

        private bool ParseParams(string[] paramVals, out object[] parsedParams, out string failOutput)
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
                        parsedParams[i] = ParamInfo[i].TypeParser.Invoke(paramVals[i]);
                    }
                    catch
                    {
                        parsedParams = null;
                        failOutput = $"Could not parse `{paramVals[i]}` when executing command.";
                        return false;
                    }

                    continue;
                }

                if(!ParamInfo[i].IsOptional)
                {
                    parsedParams = null;
                    failOutput = $"Parameter at position `{i}` of type `{ParamInfo[i].Type.Name}` must be specified.";
                    return false;
                }
            }

            return true;
        }
    }

    internal struct ParamInfo
    {
        internal readonly Type Type;
        internal readonly Func<string, object> TypeParser;
        internal readonly bool IsOptional;

        internal ParamInfo(Type type, Func<string, object> typeParser, bool isOptionalParam = false)
        {
            Type = type;
            TypeParser = typeParser;
            IsOptional = isOptionalParam;
        }
    }
}
