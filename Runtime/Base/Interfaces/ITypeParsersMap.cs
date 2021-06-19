using System;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Interface to be implemented by a class that maps parsers to their Types to parse. The implementation should be able to get a parser for a specific object Type. The class should be able to retrieve a instance
    /// of <see cref="Func{string, object}<"/> which will act as the parser, where `string` is the specific object that needs parsing, but in string form,
    /// and where `object` is the object that will be returned on a successful parse.<br/><br/>
    /// 
    /// <see cref="ITypeParsersMap"/> is not inherently used within the `SimpleCommands` base project, but is one of the main features for the default implementation of command
    /// retrieval in the implementation of the <see cref="ICommandMap"/>. However, this is not neccessery if a custom solution is used for parsing and retrieving commands
    /// based on input.
    /// </summary>
    public interface ITypeParsersMap
    {

        /// <summary>
        /// Try and get the parser for the specified <see cref="Type"/>. The parser is represented in the form of <see cref="Func{string, object}"/>
        /// where `string` is the specific object that needs parsing, but in string form,and where `object` is the object that will be returned on a successful parse.
        /// </summary>
        /// <param name="typeToParse">Type trying to parse.</param>
        /// <param name="paramParser">Instance of parser to return.</param>
        /// <returns>True if the parser has been found.</returns>
        bool GetParser(Type typeToParse, out Func<string, object> paramParser);
    }
}
