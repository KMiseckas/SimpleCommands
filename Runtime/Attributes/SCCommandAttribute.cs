using System;

[AttributeUsage(AttributeTargets.Method)]
public class SCCommandAttribute : Attribute
{
    public readonly string CommandKey;
    public readonly string CommandDescription;

    public SCCommandAttribute(string commandKey, string commandDescription = "")
    {
        CommandKey = commandKey.ToLower();
        CommandDescription = commandDescription;
    }
}
