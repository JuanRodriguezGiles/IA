using System;
using System.Collections.Generic;

using UnityEngine;

public class Rest : FSMAction
{
    #region PRIVATE_FIELDS
    private readonly Func<Vector2Int, Vector2Int, List<Vector2Int>> onGetPath;
    private Action<Vector2Int> onUpdateTarget;
    private readonly Func<Vector2> onGetPos;
    private Func<float> onGetDeltaTime;

    private Vector3 currentDestination;
    private readonly Vector2Int rest;
    private List<Vector2Int> path;
    private Vector2 miner;
    private int posIndex;

    private float currentRestTime = 0;
    private const float restTime = 2.0f;
    #endregion

    #region CONSTRUCTOR
    public Rest(Action<int> onSetFlag, Func<float> onGetDeltaTime, Func<Vector2> onGetPos, Func<Vector2Int, Vector2Int, List<Vector2Int>> onGetPath, Action<Vector2Int> onUpdateTarget, Vector2Int rest)
    {
        this.onSetFlag = onSetFlag;
        this.onGetDeltaTime = onGetDeltaTime;
        this.onGetPos = onGetPos;
        this.onGetPath = onGetPath;
        this.onUpdateTarget = onUpdateTarget;
        this.rest = rest;
    }
    #endregion    
    
    #region OVERRIDE
    public override void Execute()
    {
        miner = onGetPos.Invoke();

        if (path == null)
        {
            path = onGetPath.Invoke(new Vector2Int((int)miner.x, (int)miner.y), rest);

            posIndex = 0;

            currentDestination = new Vector3(path[posIndex].x, path[posIndex].y, 0);
            
            onUpdateTarget?.Invoke(new Vector2Int((int)currentDestination.x, (int)currentDestination.y));
        }
        else if (Vector2.Distance(currentDestination, miner) < 0.1f)
        {
            posIndex++;

            if (posIndex >= path.Count - 1)
            {
                if (currentRestTime < restTime)
                {
                    currentRestTime += onGetDeltaTime.Invoke();
                }
                else
                {
                    path = null;
                    onSetFlag?.Invoke((int)Flags.OnReachMine);
                    return;
                }
            }

            currentDestination = new Vector3(path[posIndex].x, path[posIndex].y, 0);
            onUpdateTarget?.Invoke(new Vector2Int((int)currentDestination.x, (int)currentDestination.y));
        }
    }
    #endregion
}