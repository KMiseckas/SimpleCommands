// MIT License 
//
// Copyright (c) 2021 Klaudijus Miseckas 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions: 
//
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
// SOFTWARE.

using System.Collections.Generic;

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Suggests the commands for auto-completion based on current user input.
    /// </summary>
    public class CommandSuggester : ITextSuggester
    {
        /// <summary>
        /// Instance of word tree (trie).
        /// </summary>
        protected static readonly Trie WORD_TRIE = new Trie();

        /// <summary>
        /// Add a collection of applicable auto-complete words to the Trie.
        /// </summary>
        /// <param name="words">Array of words as strings.</param>
        public void AddCollection(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                WORD_TRIE.Insert(words[i]);
            }
        }

        /// <summary>
        /// Get all suggestions mathching the entered prefix.
        /// </summary>
        /// <param name="prefix">Prefix to compare against.</param>
        /// <returns>Array of words matching prefix.</returns>
        public string[] GetSuggestions(string prefix)
        {
            TrieNode currentNode;

            if (!WORD_TRIE.FindPrefixNode(prefix, out currentNode) || currentNode == WORD_TRIE.Head)
            {
                return new string[0];
            }

            List<string> results = new List<string>();

            FindWordsUnderNode(currentNode, prefix, ref results);

            return results.ToArray();
        }

        /// <summary>
        /// Find all the words in the Trie saved under a single node. Iterate through the nodes recursively until all the words in the tree have been found.
        /// </summary>
        /// <param name="head">The starting node.</param>
        /// <param name="currPrefix">The prefix to match.</param>
        /// <param name="results">Results of the search.</param>
        private void FindWordsUnderNode(TrieNode head, string currPrefix, ref List<string> results)
        {
            if (head.IsLeaf())
            {
                results.Add(currPrefix);
                return;
            }

            List<TrieNode> children = head.GetChildrenNodes();

            for (int i = 0; i < children.Count; i++)
            {
                string word = currPrefix + children[i].Character;
                FindWordsUnderNode(children[i], word, ref results);
            }
        }
    }
}
