using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Attribute used to denote which methods are to be used as parsers for Types for command parameters/arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SCTypeParserAttribute : Attribute
    {
        /// <summary>
        /// The priority of this parser. Higher priority parsers override the previous parser for the same Type.
        /// </summary>
        public readonly int Priority;

        /// <summary>
        /// Create a new instance of <see cref="SCTypeParserAttribute"/>
        /// </summary>
        /// <param name="priority">The priority of this parser. Higher priority parsers override the previous parser for the same Type.</param>
        public SCTypeParserAttribute(int priority = 0)
        {
            Priority = priority;
        }
    }
}