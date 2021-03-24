using SimpleCommands.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Assertions;

namespace SimpleCommands
{
    public class CommandMap : ICommandMap
    {
        private readonly static Dictionary<string, SCCommand> _CommandMap = new Dictionary<string, SCCommand>();

        private static bool _AreStaticsInitialized;

        public CommandMap(IParsersMap parsersMap)
        {
            if(!_AreStaticsInitialized)
            {
                CreateMap(parsersMap);

                _AreStaticsInitialized = true;
            }
        }

        public bool GetCommand(string commandKey, out SCCommand command)
        {
            return _CommandMap.TryGetValue(commandKey, out command);
        }

        private void CreateMap(IParsersMap parsersMap)
        {
            MethodInfo[] commandMethods = FindMethods();

            for(int i = 0; i < commandMethods.Length; i++)
            {
                MethodInfo commandMethod = commandMethods[i];

                ParameterInfo[] methodParams = commandMethod.GetParameters();

                SCCommandAttribute attribute = commandMethod.GetCustomAttribute<SCCommandAttribute>();

                int methodParamCount = methodParams.Length;

                string commandKey = attribute.CommandKey;
                string commandDesc = attribute.CommandDescription;

                ParamInfo[] cmdParamInfo = new ParamInfo[methodParamCount];

                if(methodParams.Length > 0)
                {
                    for(int j = 0; j < methodParams.Length; j++)
                    {
                        ParameterInfo paramInfo = methodParams[j];
                        Type paramType = paramInfo.ParameterType;

                        Assert.IsTrue(parsersMap.GetParser(paramType, out Func<string, object> parserFunc), $"No parser for type `{paramType}` exists.");

                        cmdParamInfo[j] = new ParamInfo(paramType, parserFunc, paramInfo.IsOptional);
                    }
                }

                Assert.IsFalse(_CommandMap.TryGetValue(commandKey, out SCCommand command));

                SCCommand newCommand = new SCCommand(commandKey, commandDesc, cmdParamInfo, commandMethod);

                _CommandMap.Add(commandKey, newCommand);
            }
        }

        private MethodInfo[] FindMethods()
        {
            Assembly[] targetAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<MethodInfo> commandMethods = new List<MethodInfo>();

            for(int i = 0; i < targetAssemblies.Length; i++)
            {
                Type[] types = targetAssemblies[i].GetTypes();

                for(int j = 0; j < types.Length; j++)
                {
                    MethodInfo[] methods = types[j].GetMethods();

                    for(int k = 0; k < methods.Length; k++)
                    {
                        object[] attributes = methods[k].GetCustomAttributes(typeof(SCCommandAttribute), false);

                        if(attributes.Length > 0)
                        {
                            commandMethods.Add(methods[k]);
                        }
                    }
                }
            }

            return commandMethods.ToArray();
        }
    }
}
