using UnityEngine;

#region ENUMS
public enum States
{
    Mining,
    GoToMine,
    GoToDeposit,
    Idle,


    _Count
}

public enum Flags
{
    OnFullInventory,
    OnReachMine,
    OnReachDeposit,
    OnEmptyMine,


    _Count
}
#endregion

public class Miner : MonoBehaviour
{
    #region EXPOSED_FIELDS
    public GameObject mine;
    public GameObject deposit;
    #endregion
    
    #region PRIVATE_FIELDS
    private FSM fsm;
    private const float speed = 10.0f;
    private const float miningTime = 5.0f;
    private float currentMiningTime = 0.0f;
    private int mineUses = 10;
    #endregion

    private void Start()
    {
        Mine asd = new Mine(fsm.SetFlag);
        
        fsm = new FSM((int)States._Count, (int)Flags._Count);
        fsm.ForceCurrentState((int)States.GoToMine);

        fsm.SetRelation((int)States.GoToMine, (int)Flags.OnReachMine, (int)States.Mining);
        fsm.SetRelation((int)States.Mining, (int)Flags.OnFullInventory, (int)States.GoToDeposit);
        fsm.SetRelation((int)States.GoToDeposit, (int)Flags.OnReachDeposit, (int)States.GoToMine);
        fsm.SetRelation((int)States.GoToDeposit, (int)Flags.OnEmptyMine, (int)States.Idle);

        fsm.AddBehaviour((int)States.Idle, () => { Debug.Log("Idle"); });

        fsm.AddBehaviour((int)States.Mining, () =>
        {
            if (currentMiningTime < miningTime)
            {
                currentMiningTime += Time.deltaTime;
            }
            else
            {
                currentMiningTime = 0.0f;
                fsm.SetFlag((int)Flags.OnFullInventory);
                mineUses--;
            }
        });
        fsm.AddBehaviour((int)States.Mining, () => { Debug.Log("Mining"); });

        fsm.AddBehaviour((int)States.GoToMine, () =>
        {
            Vector2 dir = (mine.transform.position - transform.position).normalized;

            if (Vector2.Distance(mine.transform.position, transform.position) > 1.0f)
            {
                Vector2 movement = dir * speed * Time.deltaTime;
                transform.position += new Vector3(movement.x, movement.y);
            }
            else
            {
                fsm.SetFlag((int)Flags.OnReachMine);
            }
        });
        fsm.AddBehaviour((int)States.GoToMine, () => { Debug.Log("GoToMine"); });

        fsm.AddBehaviour((int)States.GoToDeposit, () =>
        {
            Vector2 dir = (deposit.transform.position - transform.position).normalized;

            if (Vector2.Distance(deposit.transform.position, transform.position) > 1.0f)
            {
                Vector2 movement = dir * speed * Time.deltaTime;
                transform.position += new Vector3(movement.x, movement.y);
            }
            else
            {
                if (mineUses <= 0)
                {
                    fsm.SetFlag((int)Flags.OnEmptyMine);
                }
                else
                {
                    fsm.SetFlag((int)Flags.OnReachDeposit);
                }
            }
        });
        fsm.AddBehaviour((int)States.GoToDeposit, () => { Debug.Log("GoToDeposit"); });
    }

    private void Update()
    {
        fsm.Update();
    }
}