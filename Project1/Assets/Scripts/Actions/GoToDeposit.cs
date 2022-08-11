using System;

using UnityEngine;

public class GoToDeposit : FSMAction
{
    private Transform deposit;
    private Transform miner;
    private const float speed = 10.0f;
    
    public override void Execute()
    {
        Vector2 dir = (deposit.position - miner.position).normalized;

        if (Vector2.Distance(deposit.position, miner.position) > 1.0f)
        {
            Vector2 movement = dir * speed * Time.deltaTime;
            miner.position += new Vector3(movement.x, movement.y);
        }
        else
        {
            // if (mineUses <= 0)
            // {
            //     onSetFlag?.Invoke((int)Flags.OnEmptyMine);
            // }
            // else
            {
                onSetFlag?.Invoke((int)Flags.OnReachDeposit);
            }
        }
    }

    public GoToDeposit(Action<int> onSetFlag, Transform deposit, Transform miner)
    {
        this.onSetFlag = onSetFlag;
        this.deposit = deposit;
        this.miner = miner;
    }
}