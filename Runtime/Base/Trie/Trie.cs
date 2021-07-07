// Copyright (c) 2021 Klaudijus Miseckas. All Rights Reserved

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
