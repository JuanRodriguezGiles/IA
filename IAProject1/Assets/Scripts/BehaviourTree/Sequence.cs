using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : TreeNode
{
    public Sequence()
    {
       
    }

    public Sequence(List<TreeNode> children)
    {
        
    }

    public override NodeState Evaluate()
    {
        foreach (TreeNode node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    break;
                case NodeState.SUCCESS:
                    break;
                case NodeState.FAILURE:
                    state = NodeState.FAILURE;
                    return state;
                    break;
                default:
                    break;
            }
        }
    }
}