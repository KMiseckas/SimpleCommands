namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Interface to be implemented by a class which can parse a command input string into an instance of <see cref="CommandInputInfo"/>.
    /// </summary>
    public interface ICommandInputParser
    {
        /// <summary>
        /// Try attempt to parse a command input string. If successful, return an instance of <see cref="CommandInputInfo"/> which contains the information gathered from the parsed 
        /// command string.
        /// </summary>
        /// <param name="commandInput">The command input string.</param>
        /// <param name="commandInputInfo">On success, the instance populated with parsed information.</param>
        /// <returns>True if parsing did not fail.</returns>
        bool TryParseCommandInput(string commandInput, out CommandInputInfo commandInputInfo);
    }
}
