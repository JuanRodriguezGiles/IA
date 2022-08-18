using System;

using UnityEngine;

public class GoToDeposit : FSMAction
{
    #region PRIVATE_FIELDS
    private readonly Vector3 deposit;
    private readonly Transform miner;
    private const float speed = 10.0f;
    private readonly Func<float> onGetDeltaTime;
    #endregion

    #region OVERRIDE
    public override void Execute()
    {
        Vector2 dir = (deposit - miner.position).normalized;

        if (Vector2.Distance(deposit, miner.position) > 1.0f)
        {
            Vector2 movement = dir * (speed * onGetDeltaTime.Invoke());
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
    #endregion

    #region CONSTRUCTOR
    public GoToDeposit(Action<int> onSetFlag, Func<float> onGetDeltaTime, Vector3 deposit, Transform miner)
    {
        this.onGetDeltaTime = onGetDeltaTime;
        this.onSetFlag = onSetFlag;
        this.deposit = deposit;
        this.miner = miner;
    }
    #endregion
}