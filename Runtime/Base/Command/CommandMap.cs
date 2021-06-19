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

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Default implementation of the <see cref="ICommandMap"/>. Contains a mapping between unique command key (ID) and an instance of <see cref="SCCommand"/>. Mappings are created using
    /// any attributes that have been found across the project assemblies.<br/><br/>
    /// 
    /// It is recommended to use this default implementation of <see cref="ICommandMap"/> but not neccessery if the developer was to copy/write their own method of gathering all the 
    /// <see cref="SCCommandAttribute"/>s and creating <see cref="SCCommand"/> instances from that information. In short, the system does allow for creating a custom command map if desired.
    /// </summary>
    public class CommandMap : ICommandMap
    {
        /// <summary>
        /// Dictionary mapping the unique command key (ID) to the instance of <see cref="SCCommand"/>.
        /// </summary>
        private readonly static Dictionary<string, SCCommand> _CommandMap = new Dictionary<string, SCCommand>();

        /// <summary>
        /// Are the statics of the class initialised.
        /// </summary>
        private static bool _AreStaticsInitialized;

        /// <summary>
        /// Create an instance of <see cref="CommandMap"/>.
        /// </summary>
        /// <param name="parsersMap">Instance of <see cref="ITypeParsersMap"/>.</param>
        internal protected CommandMap(ITypeParsersMap parsersMap)
        {
            if (!_AreStaticsInitialized)
            {
                CreateMap(parsersMap);

                _AreStaticsInitialized = true;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string[] GetAllCommandKeys()
        {
            ICollection collection = _CommandMap.Keys;

            var stringList = new List<string>((IEnumerable<string>)collection);

            return stringList.ToArray();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool TryGetCommand(string commandKey, out SCCommand command)
        {
            return _CommandMap.TryGetValue(commandKey, out command);
        }

        /// <summary>
        /// Create the map of commands to their unique command key (ID). The command objects are created from any <see cref="SCCommandAttribute"/>s that have been located within the 
        /// project assemblies.
        /// </summary>
        /// <param name="parsersMap">The instance of <see cref="ITypeParsersMap" implementation./></param>
        private void CreateMap(ITypeParsersMap parsersMap)
        {
            var commandMethodInfo = FindCommandMethodInfo();

            //For every method information stored.
            for (var i = 0; i < commandMethodInfo.Length; i++)
            {
                var commandMethod = commandMethodInfo[i].Method;
                var methodClassType = commandMethodInfo[i].ClassType;
                var methodParams = commandMethod.GetParameters();
                var attribute = commandMethod.GetCustomAttribute<SCCommandAttribute>();

                var methodParamCount = methodParams.Length;
                var commandKey = attribute.UseMethodName ? commandMethod.Name.ToLower() : attribute.CommandKey;
                var commandDesc = attribute.CommandDescription;

                var cmdParamInfo = new ParamInfo[methodParamCount];

                if (methodParams.Length > 0)
                    for (var j = 0; j < methodParams.Length; j++)
                    {
                        var paramInfo = methodParams[j];
                        var paramType = paramInfo.ParameterType;

                        //Check if a parser for the parameter type that this method would use exists in our `Parsers Map`.
                        //If false, we cannot continue because trying to execute the command would crash due to not being able to convert a string object into a concrete type.
                        Assert.IsTrue(parsersMap.GetParser(paramType, out var parserFunc), $"No parser for type `{paramType}` exists.");

                        cmdParamInfo[j] = new ParamInfo(paramType, parserFunc, paramInfo.IsOptional);
                    }

                //Fail early incase duplicate commands have been defined.
                Assert.IsFalse(_CommandMap.TryGetValue(commandKey, out var command), $"Command Key [{commandKey}] already exists. Please make sure there are not commands with the same name.");

                var newCommand = new SCCommand(commandKey, commandDesc, methodClassType, cmdParamInfo, commandMethod);

                _CommandMap.Add(commandKey, newCommand);
            }
        }

        /// <summary>
        /// Use reflection to find all methods that contain the <see cref="SCCommandAttribute"/> usages and store the information into an array for these methods.
        /// </summary>
        /// <returns>Array of <see cref="CommandMethodInfo"/> objects.</returns>
        private CommandMethodInfo[] FindCommandMethodInfo()
        {
            //Get all the assemblies to scan.
            var targetAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var commandMethods = new List<CommandMethodInfo>();

            //For every assembly, get every possible class type.
            for (var i = 0; i < targetAssemblies.Length; i++)
            {
                var types = targetAssemblies[i].GetTypes();

                //For every class type found, get every method.
                for (var j = 0; j < types.Length; j++)
                {
                    try
                    {
                        var methods = types[j].GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

                        //For every method found, check if the method has a command attribute defined for it.
                        for (var k = 0; k < methods.Length; k++)
                        {
                            var attribute = methods[k].GetCustomAttribute<SCCommandAttribute>();

                            //If attribute exists, add it to the list of method info that contain an attribute.
                            if (attribute != null && attribute.Include)
                                commandMethods.Add(new CommandMethodInfo(methods[k], types[j]));
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }

            return commandMethods.ToArray();
        }

        /// <summary>
        /// Simple data class to store the <see cref="MethodInfo"/> and the <see cref="Type"/> of class within which it is located.
        /// </summary>
        private struct CommandMethodInfo
        {
            /// <summary>
            /// Method information of the method on which the attribute is defined on.
            /// </summary>
            internal readonly MethodInfo Method;

            /// <summary>
            /// The type of class this method with the attribute is located.
            /// </summary>
            internal readonly Type ClassType;

            /// <summary>
            /// Create a new instance of <see cref="CommandMethodInfo"/>.
            /// </summary>
            /// <param name="method">Method info.</param>
            /// <param name="classType">Type of class method is located in.</param>
            internal CommandMethodInfo(MethodInfo method, Type classType)
            {
                Method = method;
                ClassType = classType;
            }
        }
    }
}
