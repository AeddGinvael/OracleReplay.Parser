using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Oracle.Utilities.Collections;

namespace Oracle.Packets.Fields
{
    public static class FieldOpHuffmanTree
    {
        public static Node Root;
        public static int[][] Tree;
        public static FieldOpType[] Ops = FieldOpType.GetValues();

        static FieldOpHuffmanTree()
        {
            Root = BuildTree();
            var akku = new List<int[]>();
            BuildFixedTreeR(akku, Root);
            Tree = ReverseTree(akku);
        }


        private static Node BuildTree()
        {
            
            int n = 0;
            FieldOpType[] var2 = Ops;
            int var3 = var2.Length;
            var queue = new PriorityListQueue<Node>(var3);
            

            for (int var4 = 0; var4 < var3; ++var4)
            {
                FieldOpType op = var2[var4];
                var leafNode = (Node)new LeafNode(op, n++); 
                queue.Enqueue(leafNode);
            }

            while (queue.Count > 1)
            {
                var node1 = (Node) queue.Dequeue();
                var node2 = (Node) queue.Dequeue();
                var internalNode = (Node)new InternalNode(node1, node2, n++);
                queue.Enqueue(internalNode);
            }

            return queue.Peek();
        }

        private static int BuildFixedTreeR(List<int[]> akku, Node n)
        {
            akku.Add(
                new int[]
                {
                    (n.Left is LeafNode) ? - n.Left.op.ordinal - 1: BuildFixedTreeR(akku, n.Left),
                    (n.Right is LeafNode) ? - n.Right.op.ordinal - 1 : BuildFixedTreeR(akku, n.Right)
                }
            );

            return akku.Count - 1;
        }

        private static int[][] ReverseTree(List<int[]> akku)
        {
            var r = akku.Count - 1;
            var reverse = new int[r + 1][];
            for (int i = 0; i < r + 1; i++)
            {
                reverse[i] = new int[2];
            }
            for (int i = 0; i <= r; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    int s = akku[r - i][j];
                    reverse[i][j] = s < 0 ? s : r - s;

                }
            }

            return reverse;
        }
        public class LeafNode : Node
        {
            public LeafNode(FieldOpType op, int num) : base(Math.Max(op.Weight, 1), num)
            {
                this.op = op;
            }
        }
        public class InternalNode : Node
        {
            public InternalNode(Node left, Node right, int num) : base(left.weight + right.weight, num)
            {
                this.Left = left;
                this.Right = right;
            }

            public override string ToString()
            {
                return $"({weight})";
            }
        }

        public abstract class Node : IComparable<Node>
        {
            public  int weight;
            public  int num;
            public  FieldOpType op;
            public  Node Left;
            public  Node Right;

            public Node(int weight, int num)
            {
                this.weight = weight;
                this.num = num;

            }

            public int CompareTo(Node other)
            {
                int r = weight.CompareTo(other.weight);
                return r != 0 ? r : other.num.CompareTo(num);

            }
        }

    }
}
