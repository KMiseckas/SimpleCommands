using System;

namespace SimpleCommands.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SCCommandAttribute : Attribute
    {
        public readonly string CommandKey;
        public readonly string CommandDescription;
        public CommandTargetType CommandTargetType;

        public SCCommandAttribute(string commandKey, string commandDescription = "")
        {
            CommandKey = commandKey.ToLower();
            CommandDescription = commandDescription;
        }
    }

    public enum CommandTargetType
    {
        Mono,
        Class,
        Static
    }
}
