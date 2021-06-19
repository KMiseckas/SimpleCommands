namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Interface to be implemented by a class that provides possible words from the provided prefix from a collection of applicable words.
    /// </summary>
    public interface ITextSuggester
    {
        /// <summary>
        /// Add a collection of text/words from which the suggestions will be picked from. These should be all the words that can be auto completed by the implementation 
        /// of this interface.
        /// </summary>
        /// <param name="words">Array of words that can be suggested for auto-completion.</param>
        void AddCollection(string[] words);

        /// <summary>
        /// Get the suggestions for auto completion based on the prefix provided.
        /// </summary>
        /// <param name="prefix">The prefix of a word for which to search suggestions for.</param>
        /// <returns>An array limited of words/text that have the same prefix.</returns>
        string[] GetSuggestions(string prefix);
    }
}
