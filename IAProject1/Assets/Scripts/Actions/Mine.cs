using System;

public class Mine : FSMAction
{
    #region CONSTRUCTOR
    public Mine(Action<int> onSetFlag, Func<float> onGetDeltaTime)
    {
        this.onGetDeltaTime = onGetDeltaTime;
        this.onSetFlag = onSetFlag;
    }
    #endregion

    #region PRIVATE_FIELDS
    private readonly Func<float> onGetDeltaTime;

    private float currentMiningTime = 0;
    private int mineUses = 10;

    private const float miningTime = 2.0f;
    #endregion

    #region OVERRIDE
    public override void Execute()
    {
        if (currentMiningTime < miningTime)
        {
            currentMiningTime += onGetDeltaTime.Invoke();
        }
        else
        {
            currentMiningTime = 0.0f;
            onSetFlag?.Invoke((int)Flags.OnFullInventory);
            mineUses--;
        }
    }
    #endregion
}