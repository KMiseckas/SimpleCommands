// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

using SimpleCommands.Runtime.Base;

namespace SimpleCommands.Runtime.Implementations
{
    /// <summary>
    /// Extension of the base class for simple commands. Implements the abstract methods required for `SimpleCommands` functionality to work. 
    /// - Creates an implementation of a <see cref="ICommandMap"/>, by default this is an instance of <see cref="CommandMap"/>.<br/>
    /// - Creates an implementation of a <see cref="ICommandInputParser"/>, by default this is an instance of <see cref="CommandInputParser"/>.<br/>
    /// - Creates an additional implementation of <see cref="ITypeParsersMap"/> which by default is an instance of <see cref="TypeParsersMap"/>.
    /// <br/><br/>
    /// The method of <see cref="CreateParserMap"/> can be overriden in an extended class to return a custom <see cref="ITypeParsersMap"/> implementation. It is recommended to 
    /// extend the class of <see cref="TypeParsersMap"/> and add your own parsers for any object type you may want to be able to parse through a command execution (for example, 
    /// you may want to add a parser for a Vector4"/>.
    /// <br/><br/>
    /// For more info see <see cref="TypeParsersMap"/>.
    /// <br/><br/>
    /// </summary>
    public class SCDefault : SCBase
    {
    }
}
