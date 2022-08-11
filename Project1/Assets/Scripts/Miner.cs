using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

public class Miner : MonoBehaviour
{
    #region EXPOSED_FIELDS
    #endregion

    #region PRIVATE_FIELDS
    private FSM fsm;
    private GameObject mine;
    private GameObject deposit;
    #endregion

    public void Init(GameObject mine, GameObject deposit)
    {
        this.mine = mine;
        this.deposit = deposit;

        fsm = new FSM((int)States._Count, (int)Flags._Count);
        fsm.ForceCurrentState((int)States.GoToMine);

        fsm.SetRelation((int)States.GoToMine, (int)Flags.OnReachMine, (int)States.Mining);
        fsm.SetRelation((int)States.Mining, (int)Flags.OnFullInventory, (int)States.GoToDeposit);
        fsm.SetRelation((int)States.GoToDeposit, (int)Flags.OnReachDeposit, (int)States.GoToMine);
        fsm.SetRelation((int)States.GoToDeposit, (int)Flags.OnEmptyMine, (int)States.Idle);

        fsm.AddBehaviour((int)States.Idle, new Idle(fsm.SetFlag));
        fsm.AddBehaviour((int)States.Mining, new Mine(fsm.SetFlag));
        fsm.AddBehaviour((int)States.GoToMine, new GoToMine(fsm.SetFlag, mine.transform, transform));
        fsm.AddBehaviour((int)States.GoToDeposit, new GoToDeposit(fsm.SetFlag, deposit.transform, transform));
    }

    public void UpdateMiner()
    {
        fsm.Update();
    }
}