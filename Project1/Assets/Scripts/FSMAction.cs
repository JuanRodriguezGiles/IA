using System;

public abstract class FSMAction
{
    protected Action<int> onSetFlag;
    public abstract void Execute();
}