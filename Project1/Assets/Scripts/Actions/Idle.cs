using System;

using UnityEngine;

public class Idle : FSMAction
{
    public override void Execute()
    {
       Debug.Log("Idle");
    }

    public Idle(Action<int> onSetFlag)
    {
        this.onSetFlag = onSetFlag;
    }
}