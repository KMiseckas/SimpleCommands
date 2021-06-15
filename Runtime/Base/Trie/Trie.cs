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

namespace SimpleCommands.Runtime.Base
{
    /// <summary>
    /// Tree structure that holds characters which make up words as you go down the tree.
    /// </summary>
    public class Trie
    {
        /// <summary>
        /// Head node of the trie.
        /// </summary>
        public readonly TrieNode Head;

        /// <summary>
        /// Create a new instance of <see cref="Trie"/>.
        /// </summary>
        public Trie()
        {
            Head = new TrieNode('-', 0, null);
        }

        /// <summary>
        /// Find a node which will contain the words under the passed in prefix.
        /// </summary>
        /// <param name="prefix">The prefix to match.</param>
        /// <param name="lastFoundNode">The node found which matches the prefix.</param>
        /// <returns>True if a node has been found for the passed in prefix.</returns>
        public bool FindPrefixNode(string prefix, out TrieNode lastFoundNode)
        {
            TrieNode iteratedNode = Head;

            for (int i = 0; i < prefix.Length; i++)
            {
                if (!iteratedNode.TryGetChildNode(prefix[i], out TrieNode foundNode))
                {
                    lastFoundNode = iteratedNode;
                    return false;
                }

                iteratedNode = foundNode;
            }

            lastFoundNode = iteratedNode;
            return true;
        }

        /// <summary>
        /// Insert a word into the trie.
        /// </summary>
        /// <param name="word">Word as a string.</param>
        public void Insert(string word)
        {
            TrieNode iteratedNode;

            FindPrefixNode(word, out iteratedNode);

            for (int i = iteratedNode.Depth; i < word.Length; i++)
            {
                TrieNode newNode = new TrieNode(word[i], iteratedNode.Depth + 1, iteratedNode);
                iteratedNode.AddChildNode(newNode);
                iteratedNode = newNode;
            }
        }
    }
}
