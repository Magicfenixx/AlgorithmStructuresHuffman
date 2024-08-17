using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanTree
{
    internal class HuffmanTree
    {
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; }
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();

        public void Build(string source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!Frequencies.ContainsKey(source[i]))
                {
                    Frequencies.Add(source[i], 0);
                }

                Frequencies[source[i]]++;
            }

            foreach (KeyValuePair<char, int> symbol in Frequencies)
            {
                nodes.Add(new Node() { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            while (nodes.Count > 1)
            {
                List<Node> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<Node>();

                if (orderedNodes.Count >= 2)
                {
                    // Take first two items
                    List<Node> taken = orderedNodes.Take(2).ToList<Node>();

                    // Create a parent node by combining the frequencies
                    Node parent = new Node()
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                this.Root = nodes.FirstOrDefault();

            }

        }
        public static KeyValuePair<char, string> getCodeForCurrentElement(char symbol, List<bool> encodedSymbol, Dictionary<char, string> encoding_table)
        {
            
            string code = "";
            foreach (var boolean in encodedSymbol)
            {
                code += boolean ? 1 : 0;
            }
            KeyValuePair<char, string> symbol_with_code = new KeyValuePair<char, string>(symbol, code);
            return symbol_with_code;
        }

        public BitArray Encode(string source)
        {
            List<bool> encodedSource = new List<bool>();
            Dictionary<char, string> encoding_table = new Dictionary<char, string>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());

                encodedSource.AddRange(encodedSymbol);

                var symbol_with_code = getCodeForCurrentElement(source[i], encodedSymbol, encoding_table);
                encoding_table.TryAdd(symbol_with_code.Key, symbol_with_code.Value);
            }

            Console.WriteLine("\nEncoded symbols:");
            foreach(var pair in encoding_table)
            {
                Console.WriteLine(pair.Key + " | " + pair.Value);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());


            return bits;
        }

        public string Decode(BitArray bits)
        {
            Node current = this.Root;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = this.Root;
                }
            }

            return decoded;
        }

        public bool IsLeaf(Node node)
        {
            return (node.Left == null && node.Right == null);
        }

    }
}
