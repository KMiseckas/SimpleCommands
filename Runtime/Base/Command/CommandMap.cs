// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public class CommandMap
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
        internal protected CommandMap(TypeParsersMap parsersMap)
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
        protected virtual void CreateMap(TypeParsersMap parsersMap)
        {
            var commandMethodInfo = ReflectionUtils.FindAttributeMethodInfo<SCCommandAttribute>(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            //For every method information stored.
            for (var i = 0; i < commandMethodInfo.Length; i++)
            {
                SCCommandAttribute attribute = commandMethodInfo[i].Attribute;

                if (!attribute.Include) 
                    continue;

                var commandMethod = commandMethodInfo[i].MethodInfo;
                var methodClassType = commandMethod.DeclaringType;
                var methodParams = commandMethod.GetParameters();

                var methodParamCount = methodParams.Length;
                var commandKey = attribute.UseMethodName ? commandMethod.Name.ToLower() : attribute.CommandKey;
                var commandDesc = attribute.CommandDescription;

                var cmdParamInfo = new ParamInfo[methodParamCount];

                if (methodParams.Length > 0)
                {
                    for (var j = 0; j < methodParams.Length; j++)
                    {
                        var paramInfo = methodParams[j];
                        var paramType = paramInfo.ParameterType;

                        //Check if a parser for the parameter type that this method would use exists in our `Parsers Map`.
                        //If false, we cannot continue because trying to execute the command would crash due to not being able to convert a string object into a concrete type.
                        /*Assert.IsTrue(*/
                        parsersMap.GetParser(paramType, out var parserFunc);/*, $"No parser for type `{paramType}` exists.");*/

                        cmdParamInfo[j] = new ParamInfo(paramType, parserFunc, paramInfo.IsOptional);
                    }
                }

                //Fail early incase duplicate commands have been defined.
                Assert.IsFalse(_CommandMap.TryGetValue(commandKey, out var command), $"Command Key [{commandKey}] already exists. Please make sure there are not commands with the same name.");

                var newCommand = new SCCommand(commandKey, commandDesc, methodClassType, cmdParamInfo, commandMethod);

                _CommandMap.Add(commandKey, newCommand);
            }
        }
    }
}
