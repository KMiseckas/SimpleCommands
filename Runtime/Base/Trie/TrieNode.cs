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
