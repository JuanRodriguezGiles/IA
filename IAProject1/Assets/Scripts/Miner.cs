using System;

using UnityEngine;

public class Miner : MonoBehaviour
{
    #region PRIVATE_FIELDS
    private FSM fsm;
    #endregion

    #region PUBLIC_METHODS
    public void Init(GameObject mine, GameObject deposit, Func<float> onGetDeltaTime)
    {
        fsm = new FSM((int)States._Count, (int)Flags._Count);
        fsm.ForceCurrentState((int)States.GoToMine);

        fsm.SetRelation((int)States.GoToMine, (int)Flags.OnReachMine, (int)States.Mining);
        fsm.SetRelation((int)States.Mining, (int)Flags.OnFullInventory, (int)States.GoToDeposit);
        fsm.SetRelation((int)States.GoToDeposit, (int)Flags.OnReachDeposit, (int)States.GoToMine);
        fsm.SetRelation((int)States.GoToDeposit, (int)Flags.OnEmptyMine, (int)States.Idle);

        fsm.AddBehaviour((int)States.Idle, new Idle(fsm.SetFlag));
        fsm.AddBehaviour((int)States.Mining, new Mine(fsm.SetFlag, onGetDeltaTime));
        fsm.AddBehaviour((int)States.GoToMine, new GoToMine(fsm.SetFlag, onGetDeltaTime, mine.transform.position, transform));
        fsm.AddBehaviour((int)States.GoToDeposit, new GoToDeposit(fsm.SetFlag, onGetDeltaTime, deposit.transform.position, transform));
    }

    public void UpdateMiner()
    {
        fsm.Update();
    }
    #endregion
  
}