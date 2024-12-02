﻿using System;
using System.Collections.Generic;

namespace Ganss.Text
{
    /// <summary>
    /// A <a href="https://en.wikipedia.org/wiki/Trie">Trie</a>.
    /// </summary>
    [Serializable]
    public class Trie
    {
        
        /// <summary>
        /// The Highest ID number used in this Trie hierarchy
        /// </summary>
        public static long LatestID = 0;
        
        /// <summary>
        /// has this entry been saved yet.
        /// </summary>
        public bool hasSaved;

        /// <summary>
        /// The unique long integer ID of this entry
        /// </summary>
        public long uniqueID;

        /// <summary>
        /// Which comparer was used 
        /// </summary>
        public IEqualityComparer<char>  usedComparer;
        
        /// <summary>
        /// Gets or sets the child nodes.
        /// </summary>
        /// <value>
        /// The child nodes.
        /// </value>
        public Dictionary<char, Trie> Next { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance represents a word in the dictionary.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a word in the dictionary; otherwise, <c>false</c>.
        /// </value>
        public bool IsWord { get; set; }

        /// <summary>
        /// Gets or sets the failure node.
        /// </summary>
        /// <value>
        /// The failure node.
        /// </value>
        public Trie Fail { get; set; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public Trie Parent { get; set; }

        /// <summary>
        /// Gets the word prefix this node represents.
        /// </summary>
        /// <value>
        /// The word prefix.
        /// </value>
        public string Word { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trie"/> class.
        /// </summary>
        public Trie()
        {
            Word = "";
            Next = new Dictionary<char, Trie>();
            LatestID += 1;
            uniqueID = LatestID;
            hasSaved = false; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trie"/> class.
        /// </summary>
        /// <param name="comparer">The comparer used to compare individual characters.</param>
        public Trie(IEqualityComparer<char> comparer)
        {
            Word = "";
            Next = new Dictionary<char, Trie>(comparer);
            usedComparer = comparer;
            LatestID += 1;
            uniqueID = LatestID;
            hasSaved = false;
        }

        /// <summary>
        /// Adds the specified word to the trie.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public virtual Trie Add(string word)
        {
            var c = word[0];

            if (!Next.TryGetValue(c, out Trie node))
                Next[c] = node = new Trie(Next.Comparer) { Parent = this, Word = Word + c };

            if (word.Length > 1)
                return node.Add(word.Substring(1));
            else
                node.IsWord = true;

            return node;
        }
     

        /// <summary>
        /// Finds the failure node for a specified suffix within the given range of indices.
        /// </summary>
        /// <param name="word">The string containing the suffix.</param>
        /// <param name="startIndex">The start index of the suffix within the string.</param>
        /// <param name="endIndex">The end index (exclusive) of the suffix within the string.</param>
        /// <returns>The failure node or null if no failure node is found.</returns>

        public virtual Trie ExploreFailLink(string word, int startIndex, int endIndex)
        {
            var node = this;

            for (int i = startIndex; i < endIndex; i++)
            {
                if (!node.Next.TryGetValue(word[i], out node))
                {
                    return null;
                }
            }

            return node;
        }
    }
}
