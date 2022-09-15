using System;
using System.Collections.Generic;

using UnityEngine;

public class Miner : MonoBehaviour
{
    #region PRIVATE_FIELDS
    private FlockingMiners flockingMiners;
    private Pathfinding pathfinding;
    private Node[] map;
    private FSM fsm;
    #endregion

    #region EXPOSED_FIELDS
    public Vector2 currentPos;
    public bool updatePos;
    #endregion

    #region PUBLIC_METHODS
    public void Init(Vector2Int mine, Vector2Int deposit, Vector2Int rest, Vector2 currentPos, Func<float> onGetDeltaTime)
    {
        map = new Node[50 * 50];
        NodeUtils.MapSize = new Vector2Int(50, 50);
        var id = 0;

        for (var i = 0; i < 50; i++)
        {
            for (var j = 0; j < 50; j++)
            {
                map[id] = new Node(id, new Vector2Int(j, i));
                id++;
            }
        }

        pathfinding = new Pathfinding();
        this.currentPos = currentPos;
        //--------------------------------------------------------------------------------
        fsm = new FSM((int)States._Count, (int)Flags._Count);

        fsm.SetRelation((int)States.GoToMine, (int)Flags.OnReachMine, (int)States.Mining);
        fsm.SetRelation((int)States.Mining, (int)Flags.OnFullInventory, (int)States.GoToDeposit);
        fsm.SetRelation((int)States.GoToDeposit, (int)Flags.OnReachDeposit, (int)States.GoToMine);
        fsm.SetRelation((int)States.GoToDeposit, (int)Flags.OnEmptyMine, (int)States.Idle);
        
        fsm.SetRelation((int)States.Mining, (int)Flags.OnRest, (int)States.Resting);
        fsm.SetRelation((int)States.Resting, (int)Flags.OnFinishedResting, (int)States.GoToMine);

        fsm.AddBehaviour((int)States.Idle, new Idle(fsm.SetFlag));
        fsm.AddBehaviour((int)States.Mining, new Mine(fsm.SetFlag, onGetDeltaTime), () => { fsm.SetFlag((int)Flags.OnRest); });
        fsm.AddBehaviour((int)States.GoToMine, new GoToMine(fsm.SetFlag, GetPos, GetPath, UpdateTarget, mine));
        fsm.AddBehaviour((int)States.GoToDeposit, new GoToDeposit(fsm.SetFlag, GetPos, GetPath, UpdateTarget, deposit));
        fsm.AddBehaviour((int)States.Resting, new Rest(fsm.SetFlag, onGetDeltaTime, GetPos, GetPath, UpdateTarget, rest));

        fsm.ForceCurrentState((int)States.GoToMine);
        //--------------------------------------------------------------------------------
        flockingMiners = GetComponent<FlockingMiners>();
        flockingMiners.Init(UpdatePos, GetPos);
    }

    public void UpdateMiner()
    {
        fsm.Update();
    }

    public void ExitMiner()
    {
        fsm.Exit();
    }
    #endregion

    #region PRIVATE_METHODS
    private void UpdatePos(Vector2 newPos)
    {
        updatePos = true;
        currentPos = newPos;
    }

    private void UpdateTarget(Vector2Int newTarget)
    {
        flockingMiners.ToggleFlocking(true);
        flockingMiners.UpdateTarget(newTarget);
    }

    private Vector2 GetPos()
    {
        return currentPos;
    }

    private List<Vector2Int> GetPath(Vector2Int origin, Vector2Int destination)
    {
        return pathfinding.GetPath(map, map[NodeUtils.PositionToIndex(origin)], map[NodeUtils.PositionToIndex(destination)]);
    }
    #endregion
}