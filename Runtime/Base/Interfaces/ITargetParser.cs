using System;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Interface to be implemented by a class that maps parsers of different target IDs types.<br/><br/>
    /// 
    ///  Parsers should be of type Func<SCCommand, string, object[]>, where the func will output the list of objects that the specific target string is trying to target for a command.
    ///
    /// </summary>
    public interface ITargetParser
    {
        /// <summary>
        /// Try to get a parser for the id type that is provided.
        /// </summary>
        /// <param name="idType">The type of ID that is being provided to the parser.</param>
        /// <param name="targetObjectParser">Object to output.</param>
        /// <returns>If succesfull, an instance of a parser for the target ID type provided.</returns>
        bool TryGetParser(string idType, out Func<SCCommand, string, object[]> targetObjectParser);
    }
}
