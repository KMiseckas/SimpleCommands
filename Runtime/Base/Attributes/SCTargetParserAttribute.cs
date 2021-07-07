// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using System;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Attribute used to denote which methods are to be used as parsers for Targets for commands.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class SCTargetParserAttribute : Attribute
    {
        /// <summary>
        /// The priority of this parser. Higher priority parsers override the previous parser for the same Target.
        /// </summary>
        public readonly int Priority;

        /// <summary>
        /// The unique key for the target parser. Will also have to be used as a key when giving a target when issuing a command i.e. [key=targetVal].
        /// </summary>
        public readonly string Key;

        /// <summary>
        /// Create a new instance of <see cref="SCTargetParserAttribute"/>
        /// </summary>
        /// <param name="priority">The priority of this parser. Higher priority parsers override the previous parser for the same Target.</param>
        /// <param name="key">The unique key for the target parser. Will also have to be used as a key when giving a target when issuing a command i.e. [key=targetVal].</param>
        public SCTargetParserAttribute(string key, int priority = 0)
        {
            Key = key;
            Priority = priority;
        }
    }
}