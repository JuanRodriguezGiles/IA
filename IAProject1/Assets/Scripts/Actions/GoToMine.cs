using System;

using UnityEngine;

public class GoToMine : FSMAction
{
    #region PRIVATE_FIELDS
    private readonly Vector3 mine;
    private readonly Transform miner;
    private const float speed = 10.0f;
    private readonly Func<float> onGetDeltaTime;
    #endregion

    #region OVERRIDE
    public override void Execute()
    {
        Vector2 dir = (mine - miner.position).normalized;

        if (Vector2.Distance(mine, miner.position) > 1.0f)
        {
            Vector2 movement = dir * (speed * onGetDeltaTime.Invoke());
            miner.position += new Vector3(movement.x, movement.y);
        }
        else
        {
            onSetFlag?.Invoke((int)Flags.OnReachMine);
        }
    }
    #endregion

    #region CONSTRUCTOR
    public GoToMine(Action<int> onSetFlag, Func<float> onGetDeltaTime, Vector3 mine, Transform miner)
    {
        this.onGetDeltaTime = onGetDeltaTime;
        this.onSetFlag = onSetFlag;
        this.mine = mine;
        this.miner = miner;
    }
    #endregion
}