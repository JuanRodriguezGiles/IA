using UnityEngine;

public class Leave : Node
{
    private int counter = 0;
    public override NodeState Evaluate()
    {
        if (counter > 3000)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            counter++;
            Debug.Log(counter);
            return NodeState.RUNNING;
        }
    }
}