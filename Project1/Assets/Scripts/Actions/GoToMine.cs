using System;

using UnityEngine;

public class GoToMine : FSMAction
{
    private Transform mine;
    private Transform miner;
    private const float speed = 10.0f;

    public override void Execute()
    {
        Vector2 dir = (mine.position - miner.position).normalized;

        if (Vector2.Distance(mine.position, miner.position) > 1.0f)
        {
            Vector2 movement = dir * speed * Time.deltaTime;
            miner.position += new Vector3(movement.x, movement.y);
        }
        else
        {
            onSetFlag?.Invoke((int)Flags.OnReachMine);
        }
    }

    public GoToMine(Action<int> onSetFlag, Transform mine, Transform miner)
    {
        this.onSetFlag = onSetFlag;
        this.mine = mine;
        this.miner = miner;
    }
}