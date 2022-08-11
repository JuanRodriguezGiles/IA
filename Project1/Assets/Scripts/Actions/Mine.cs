using System;

using UnityEngine;

public class Mine : FSMAction
{
    private const float miningTime = 5.0f;
    private float currentMiningTime = 0.0f;
    private int mineUses = 10;
    
    public override void Execute()
    {
        if (currentMiningTime < miningTime)
        {
            currentMiningTime += Time.deltaTime;
        }
        else
        {
            currentMiningTime = 0.0f;
            onSetFlag?.Invoke((int)Flags.OnFullInventory);
            mineUses--;
        }
    }

    public Mine(Action<int> onSetFlag)
    {
        this.onSetFlag = onSetFlag;
    }
}