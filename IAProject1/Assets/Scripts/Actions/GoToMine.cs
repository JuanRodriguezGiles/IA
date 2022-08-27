using System;
using System.Collections.Generic;

using UnityEngine;

public class GoToMine : FSMAction
{
    #region CONSTRUCTOR
    public GoToMine(Action<int> onSetFlag, Func<float> onGetDeltaTime, Action<Vector3> onUpdatePos, Func<Vector3> onGetPos, Func<Vector2Int, Vector2Int, List<Vector2Int>> onGetPath, Vector3 mine)
    {
        this.onGetPath = onGetPath;
        this.onGetPos = onGetPos;
        this.onGetDeltaTime = onGetDeltaTime;
        this.onUpdatePos = onUpdatePos;
        this.onSetFlag = onSetFlag;
        this.mine = mine;
    }
    #endregion

    #region PRIVATE_FIELDS
    private readonly Func<Vector2Int, Vector2Int, List<Vector2Int>> onGetPath;
    private readonly Action<Vector3> onUpdatePos;
    private readonly Func<float> onGetDeltaTime;
    private readonly Func<Vector3> onGetPos;

    private readonly Vector3 mine;
    private List<Vector2Int> path;
    private Vector3 miner;
    private Vector3 currentDestination;
    private int posIndex;

    private const float speed = 10.0f;
    #endregion

    #region OVERRIDE
    public override void Execute()
    {
        if (path == null)
        {
            miner = onGetPos.Invoke();

            path = onGetPath.Invoke(new Vector2Int((int)miner.x, (int)miner.y), new Vector2Int((int)mine.x, (int)mine.y));

            posIndex = 0;

            currentDestination = new Vector3(path[posIndex].x, path[posIndex].y, 0);
        }
        else
        {
            Vector2 dir = (currentDestination - miner).normalized;

            if (Vector2.Distance(currentDestination, miner) > 1.0f)
            {
                var movement = dir * (speed * onGetDeltaTime.Invoke());
                miner += new Vector3(movement.x, movement.y);
                onUpdatePos?.Invoke(miner);
            }
            else if (posIndex >= path.Count - 1)
            {
                path = null;
                onSetFlag?.Invoke((int)Flags.OnReachMine);
            }
            else
            {
                posIndex++;
                currentDestination = new Vector3(path[posIndex].x, path[posIndex].y, 0);
            }
        }
    }
    #endregion
}