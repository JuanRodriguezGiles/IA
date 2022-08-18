using System;

using UnityEngine;

public class Rest : FSMAction
{
    #region PRIVATE_FIELDS
    private Transform miner;
    private Func<float> onGetDeltaTime;
    private float currentRestTime = 0.0f;
    private readonly Vector3 restPos;

    private const float restTime = 2.0f;
    private const float speed = 10.0f;
    #endregion

    #region OVERRIDE
    public override void Execute()
    {
        Vector2 dir = (restPos - miner.position).normalized;

        if (Vector2.Distance(restPos, miner.position) > 1.0f)
        {
            Vector2 movement = dir * (speed * onGetDeltaTime.Invoke());
            miner.position += new Vector3(movement.x, movement.y);
        }
        else if (currentRestTime < restTime)
        {
            currentRestTime += onGetDeltaTime.Invoke();
        }
        else
        {
            currentRestTime = 0;
            onSetFlag?.Invoke((int)Flags.OnFinishedResting);
        }
    }
    #endregion

    #region CONSTRUCTOR
    public Rest(Action<int> onSetFlag, Func<float> onGetDeltaTime, Vector3 restPos, Transform miner)
    {
        this.onGetDeltaTime = onGetDeltaTime;
        this.miner = miner;
        this.restPos = restPos;
        this.onSetFlag = onSetFlag;
    }
    #endregion
}