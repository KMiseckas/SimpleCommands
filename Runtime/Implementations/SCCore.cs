using SimpleCommands.Runtime.Base;

namespace SimpleCommands.Runtime.Implementations
{
    /// <summary>
    /// Extension of the base class for simple commands. Implements the abstract methods required for `SimpleCommands` functionality to work. 
    /// - Creates an implementation of a <see cref="ICommandMap"/>, by default this is an instance of <see cref="CommandMap"/>.<br/>
    /// - Creates an implementation of a <see cref="ICommandInputParser"/>, by default this is an instance of <see cref="CommandInputParser"/>.<br/>
    /// - Creates an additional implementation of <see cref="ITypeParsersMap"/> which by default is an instance of <see cref="ParsersMap"/>.
    /// <br/><br/>
    /// The method of <see cref="CreateParserMap"/> can be overriden in an extended class to return a custom <see cref="ITypeParsersMap"/> implementation. It is recommended to 
    /// extend the class of <see cref="ParsersMap"/> and add your own parsers for any object type you may want to be able to parse through a command execution (for example, 
    /// you may want to add a parser for a Vector4"/>.
    /// <br/><br/>
    /// For more info see <see cref="ParsersMap"/>.
    /// <br/><br/>
    /// </summary>
    public class SCCore : SCBase
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override ICommandMap CreateCommandMap()
        {
            return new CommandMap(CreateParserMap());
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override ICommandInputParser CreateCommandInputParser()
        {
            return new CommandInputParser();
        }

        /// <summary>
        /// Create a new implementation instance of <see cref="ITypeParsersMap"/>.
        /// </summary>
        /// <returns>New instances <see cref="ITypeParsersMap"/> implementation.</returns>
        protected virtual ITypeParsersMap CreateParserMap()
        {
            return new ParsersMap();
        }
    }
}
