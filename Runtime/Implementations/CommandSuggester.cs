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
using SimpleCommands.Base;
using SimpleCommands.Structures;

namespace SimpleCommands
{
    public class CommandSuggester : ITextSuggester
    {
        protected static readonly Trie WORD_TRIE = new Trie();

        public void AddCollection(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                WORD_TRIE.Insert(words[i]);
            }
        }

        public string[] GetSuggestions(string prefix)
        {
            TrieNode currentNode;
            
            if(!WORD_TRIE.FindPrefixNode(prefix, out currentNode) || currentNode == WORD_TRIE.Head)
            {
                return new string[0];
            }

            List<string> results = new List<string>();

            FindWordsUnderNode(currentNode, prefix, ref results);

            return results.ToArray();
        }

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
