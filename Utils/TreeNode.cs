namespace Chess_Cabs.Utils
{
    public class TreeNode
    {
        public string Value { get; set; }
        public List<TreeNode> Children { get; set; }

        public TreeNode(string value)
        {
            Value = value;
            Children = new List<TreeNode>();
        }

        public bool Contains(string move)
        {
            foreach (TreeNode node in Children)
            {
                if (node.Value == move)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
