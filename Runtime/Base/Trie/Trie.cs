using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCommands.Structures
{
    public class Trie
    {
        public readonly TrieNode Head;

        public Trie()
        {
            Head = new TrieNode('-', 0, null);
        }

        public bool FindPrefixNode(string prefix, out TrieNode lastFoundNode)
        {
            TrieNode iteratedNode = Head;

            for (int i = 0; i < prefix.Length; i++)
            {
                if(!iteratedNode.TryGetChildNode(prefix[i], out TrieNode foundNode))
                {
                    lastFoundNode = iteratedNode;
                    return false;
                }

                iteratedNode = foundNode;
            }

            lastFoundNode = iteratedNode;
            return true;
        }

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
