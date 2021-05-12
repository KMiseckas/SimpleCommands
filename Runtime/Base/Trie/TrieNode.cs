using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCommands.Structures
{
    public class TrieNode
    {
        private char _Character;
        private Dictionary<char, TrieNode> _Children;
        private TrieNode _Head;
        private int _Depth;

        public char Character => _Character;
        public int Depth => _Depth;

        internal TrieNode(char character, int depth, TrieNode parent)
        {
            _Character = character;
            _Depth = depth;
            _Head = parent;

            _Children = new Dictionary<char, TrieNode>();
        }

        /// <summary>
        /// Is this the leaf node. End of word.
        /// </summary>
        /// <returns>True if this node has no children.</returns>
        public bool IsLeaf()
        {
            return _Children.Count == 0;
        }

        public bool TryGetChildNode(char character, out TrieNode node)
        {
            return _Children.TryGetValue(character, out node);
        }

        public void AddChildNode(TrieNode node)
        {
            _Children.Add(node._Character, node);
        }

        public void DeleteChildNode(char character)
        {
            _Children.Remove(character);
        }

        /// <summary>
        /// Get the children nodes.
        /// </summary>
        /// <returns>List of nodes that are children.</returns>
        public List<TrieNode> GetChildrenNodes()
        {
            return new List<TrieNode>(_Children.Values);
        }
    }
}
