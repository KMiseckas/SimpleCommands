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
using UnityEngine;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Attribute that defines any method as a command that is compatible with this `SimpleCommands` project.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class SCCommandAttribute : Attribute
    {
        /// <summary>
        /// The unique key for the command to be issued.
        /// </summary>
        public readonly string CommandKey;

        /// <summary>
        /// The description of the command intent.
        /// </summary>
        public readonly string CommandDescription;

        /// <summary>
        /// Should the command name be the same as the method name the attribute is attached to.
        /// </summary>
        public readonly bool UseMethodName = false;

        /// <summary>
        /// Should the command be usable in this build.
        /// </summary>
        public readonly bool Include = true;

        /// <summary>
        /// Create a new instance of the <see cref="SCCommandAttribute"/>.
        /// </summary>
        /// <param name="commandKey">The unique key which will execute the command.</param>
        /// <param name="commandDescription">The description of the command intent.</param>
        /// <param name="buildTarget"> The config build for which to include this command within. Default is <see cref="BuildTarget.PRODUCTION_AND_DEVELOPMENT"/>. `Development Build` must also be
        /// ticked when building the project for the `DEVELOPMENT_ONLY` commands to be built. This setting will also be ignored when playing in EDITOR, and only takes
        /// effect on standalone builds.</param>
        public SCCommandAttribute(string commandKey = null, string commandDescription = null, BuildTarget buildTarget = BuildTarget.PRODUCTION_AND_DEVELOPMENT)
        {
#if DEVELOPMENT_BUILD && !UNITY_EDITOR
            if(BuildTarget.PRODUCTION_ONLY.Equals(buildTarget))
            {
                Include = false;
                return;
            }
#elif !UNITY_EDITOR
            if (BuildTarget.DEVELOPMENT_ONLY.Equals(buildTarget))
            {
                Include = false;
                return;
            }
#endif

            if (string.IsNullOrWhiteSpace(commandDescription))
                commandDescription = "";

            if (string.IsNullOrWhiteSpace(commandKey))
            {
                commandKey = "";
                UseMethodName = true;
            }
            else
            {
                commandKey.Trim();

                if (commandKey.Contains(" "))
                    Debug.LogError($"Cannot create a command with a commandKey that contains a space: `{commandKey}`");
            }

            CommandKey = commandKey.ToLower();
            CommandDescription = commandDescription;
        }
    }
}
