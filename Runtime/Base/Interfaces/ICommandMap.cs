namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Interface to be implemented for a class that can return an instance of <see cref="SCCommand"/> that matches a unique key (ID).
    /// The implementation is up to the developer discretion.
    /// </summary>
    public interface ICommandMap
    {
        /// <summary>
        /// Try attempt to get an instance of a command object that matches the unique command key (ID). 
        /// </summary>
        /// <param name="commandKey">The unique command key (ID).</param>
        /// <param name="command">The instance of <see cref="SCCommand"/> object to return. This object contains all relevant information on how the command should be handled.</param>
        /// <returns>True if the command exists and can be retrieved.</returns>
        bool TryGetCommand(string commandKey, out SCCommand command);

        /// <summary>
        /// Get all the keys for any usable commands.
        /// </summary>
        /// <returns>An array of all command keys that are available to be used.</returns>
        string[] GetAllCommandKeys();
    }
}
