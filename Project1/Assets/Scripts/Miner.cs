using UnityEngine;

public class Miner : MonoBehaviour
{
    #region EXPOSED_FIELDS
    public GameObject mine;
    public GameObject deposit;
    #endregion
    
    #region PRIVATE_FIELDS
    private FSM fsm;
    #endregion

    private void Start()
    {
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

    private void Update()
    {
        fsm.Update();
    }
}