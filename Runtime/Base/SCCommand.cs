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

        internal readonly ParamInfo[] ParamInfo;

        internal readonly MethodInfo Method; 

        internal SCCommand(string key, string description, ParamInfo[] paramInfo, MethodInfo method)
        {
            Assert.IsNotNull(key);
            Assert.IsNotNull(paramInfo);
            Assert.IsNotNull(method);

            CommandKey = key;
            CommandDesc = description;
            ParamInfo = paramInfo;
            Method = method;
        }

        internal bool TryExecute(string[] paramVals, out string output)
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

            try
            {
                Method.Invoke(null, parsedParams);
            }
            catch
            {
                output = $"Execution for command `{CommandKey}` has failed with unknown reason.";
                return false;
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
}
