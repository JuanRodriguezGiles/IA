using System;

public class Mine : FSMAction
{
    #region PRIVATE_FIELDS
    private const float miningTime = 5.0f;
    private float currentMiningTime = 0.0f;
    private int mineUses = 10;
    private readonly Func<float> onGetDeltaTime;
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

    #region CONSTRUCTOR
    public Mine(Action<int> onSetFlag, Func<float> onGetDeltaTime)
    {
        this.onGetDeltaTime = onGetDeltaTime;
        this.onSetFlag = onSetFlag;
    }
    #endregion
}