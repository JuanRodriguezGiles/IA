using System;

using UnityEngine;

public class Idle : FSMAction
{
    #region OVERRIDE
    public override void Execute()
    {
        Debug.Log("Idle");
    }
    #endregion

    #region CONSTRUCTOR
    public Idle(Action<int> onSetFlag)
    {
        this.onSetFlag = onSetFlag;
    }
    #endregion
}