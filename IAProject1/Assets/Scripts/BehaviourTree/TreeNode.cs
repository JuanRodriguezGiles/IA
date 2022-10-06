using System.Collections.Generic;

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}

public abstract class TreeNode
{
    protected NodeState state;
    public TreeNode parent;
    protected List<TreeNode> children = new List<TreeNode>();

    public TreeNode()
    {
        parent = null;
    }

    public TreeNode(List<TreeNode> children)
    {
        foreach (TreeNode n in children)
        {
            Attach(n);
        }
    }

    public void Attach(TreeNode node)
    {
        node.parent = this;
        children.Add(node);
    }

    public virtual NodeState Evaluate() => NodeState.FAILURE;
}