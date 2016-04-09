using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public static class Util
    {
        public static int FooInt()
        {
            return 0;
        }

        public static void FooVoid(int[] input)
        {
            var projectInput = input.Select(ProjectInt);
            var sum = projectInput.Aggregate((a, b) => a + b);
            var sum2 = projectInput.Sum();
        }

        public static int ProjectInt(int input)
        {
            // identity projection
            return input;
        }

        public static int TreeMax(TreeNode<int> node)
        {
            if (node.Left == null && node.Right == null) return node.Value;
            return Math.Max(Math.Max(node.Value, TreeMax(node.Left)), TreeMax(node.Right));
        }
    }

    public class TreeNode<T>
    {
        public T Value { get; }
        public TreeNode<T> Left { get; }
        public TreeNode<T> Right { get; }
    }
}
