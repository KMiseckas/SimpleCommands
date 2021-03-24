using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleCommands
{
    public static class Test
    {
        [SCCommand("test1")]
        public static void Test1()
        {
            Debug.LogWarning("TEST WORKS");
        }

        [SCCommand("test2")]
        public static void Test2(int a)
        {
            Debug.LogWarning("TEST WORKS");
        }

        [SCCommand("test3")]
        public static void Test3(int a, int b)
        {
            Debug.LogWarning("TEST WORKS");
        }

        [SCCommand("test4")]
        public static void Test4(int a, int b, bool c)
        {
            Debug.LogWarning("TEST WORKS");
        }

        [SCCommand("test5")]
        public static void Test5(string a, int b)
        {
            Debug.LogWarning("TEST WORKS");
        }

        [SCCommand("test6")]
        public static void Test6(string a, int b, int d = 2)
        {
            Debug.LogWarning("TEST WORKS");
        }
    }

    public class CommandDefinitions : MonoBehaviour
    {
        private readonly static Dictionary<Type, Func<string, object>> _TypeParsers = new Dictionary<Type, Func<string, object>>();

        private readonly static Dictionary<string, SCCommand> _CommandMap = new Dictionary<string, SCCommand>();

        private void Awake()
        {
            _TypeParsers.Add(typeof(int), (x) => { return int.Parse(x); });
            _TypeParsers.Add(typeof(bool), (x) => { return bool.Parse(x); });
            _TypeParsers.Add(typeof(float), (x) => { return float.Parse(x); });
            _TypeParsers.Add(typeof(double), (x) => { return double.Parse(x); });
            _TypeParsers.Add(typeof(byte), (x) => { return byte.Parse(x); });
            _TypeParsers.Add(typeof(long), (x) => { return long.Parse(x); });
            _TypeParsers.Add(typeof(uint), (x) => { return uint.Parse(x); });
            _TypeParsers.Add(typeof(string), (x) => { return x; });
            _TypeParsers.Add(typeof(char), (x) => { return char.Parse(x); });

            DefineCommands();
        }

        protected void DefineCommands()
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

                        Assert.IsTrue(_TypeParsers.TryGetValue(paramType, out Func<string, object> parser), $"No parser for type `{paramInfo.ParameterType}` exists.");

                        cmdParamInfo[j] = new ParamInfo(paramType, parser, paramInfo.IsOptional);
                    }
                }

                Assert.IsFalse(_CommandMap.TryGetValue(commandKey, out SCCommand command));//, $"Command key `{commandKey}` already exists for method `{command.Method.Name}`. Rename key for method `{commandMethod.Name}`.");

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

        public bool GetCommand(string commandKey, out SCCommand command)
        {
            return _CommandMap.TryGetValue(commandKey, out command);
        }

       /* protected void AddCommand(SCCommand commandBase, Action<SCCommand, string[]> executeParser = null)
        {
            if(executeParser == null)
            {
                Assert.IsTrue(commandBase is SCCommand, $"parse action cannot be null if the command uses Generics");
            }

            executeParser = executeParser == null ? ((command, data) => { (command as SCCommand).Execute(); }) : executeParser;

            _TypeParsersAlpha.Add(commandBase.GetType(), executeParser);
            _RegisteredCommands.Add(commandBase.CommandKey.ToLower(), commandBase);
        }

        public bool ExecuteCommand(string commandKey, string[] data, out string output)
        {
            if(!_RegisteredCommands.TryGetValue(commandKey, out SCCommand command))
            {
                output = $"Command [{commandKey}] not found.";
                return false;
            }

            if(!_TypeParsersAlpha.TryGetValue(command.GetType(), out Action<SCCommand, string[]> executeParser))
            {
                output = $"Command [{commandKey}] execution parser not found.";
                return false;
            }

            if(!(command is SCCommand) && data == null)
            {
                output = $"Command [{commandKey}] requires parameters to execute. Format is [{command.Format}]";
                return false;
            }

            try
            {
                executeParser(command, data);
            }
            catch
            {
                output = $"Command [{commandKey}] has failed parsing for an unknown reason.";
                return false;
            }

            output = "";
            return true;
        }*/

    }
}
