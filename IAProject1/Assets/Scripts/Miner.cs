using System;
using System.Collections.Generic;

using UnityEngine;

public class Miner : MonoBehaviour
{
    #region PRIVATE_FIELDS
    private FSM fsm;
    private Pathfinding pathfinding;
    private Node[] map;
    #endregion

    #region EXPOSED_FIELDS
    public bool updatePos;
    public Vector3 currentPos;
    #endregion

    #region PUBLIC_METHODS
    public void Init(GameObject mine, GameObject deposit, GameObject rest, Vector3 currentPos, Func<float> onGetDeltaTime)
    {
        map = new Node[10 * 10];
        NodeUtils.MapSize = new Vector2Int(10, 10);
        var id = 0;

        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                map[id] = new Node(id, new Vector2Int(j, i));
                id++;
            }
        }


        map[NodeUtils.PositionToIndex(new Vector2Int(0, 3))].state = Node.NodeState.Obstacle;
        map[NodeUtils.PositionToIndex(new Vector2Int(1, 3))].state = Node.NodeState.Obstacle;
        map[NodeUtils.PositionToIndex(new Vector2Int(2, 3))].state = Node.NodeState.Obstacle;
        map[NodeUtils.PositionToIndex(new Vector2Int(3, 3))].state = Node.NodeState.Obstacle;
        map[NodeUtils.PositionToIndex(new Vector2Int(4, 3))].state = Node.NodeState.Obstacle;
        map[NodeUtils.PositionToIndex(new Vector2Int(5, 3))].state = Node.NodeState.Obstacle;
        map[NodeUtils.PositionToIndex(new Vector2Int(6, 3))].state = Node.NodeState.Obstacle;
        map[NodeUtils.PositionToIndex(new Vector2Int(7, 3))].state = Node.NodeState.Obstacle;
        
        map[NodeUtils.PositionToIndex(new Vector2Int(0, 3))].state = Node.NodeState.Obstacle;
        map[NodeUtils.PositionToIndex(new Vector2Int(1, 3))].state = Node.NodeState.Obstacle;
        map[NodeUtils.PositionToIndex(new Vector2Int(2, 3))].state = Node.NodeState.Obstacle;
        
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
        fsm.AddBehaviour((int)States.GoToMine, new GoToMine(fsm.SetFlag, onGetDeltaTime, OnUpdatePos, OnGetPos, GetPath, mine.transform.position));
        fsm.AddBehaviour((int)States.GoToDeposit, new GoToDeposit(fsm.SetFlag, onGetDeltaTime, OnUpdatePos, OnGetPos, GetPath, deposit.transform.position));
        fsm.AddBehaviour((int)States.Resting, new Rest(fsm.SetFlag, onGetDeltaTime, OnUpdatePos, OnGetPos, GetPath, rest.transform.position));

        fsm.ForceCurrentState((int)States.GoToMine);
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
    private void OnUpdatePos(Vector3 newPos)
    {
        updatePos = true;
        currentPos = newPos;
    }

    private Vector3 OnGetPos()
    {
        return currentPos;
    }

    private List<Vector2Int> GetPath(Vector2Int origin, Vector2Int destination)
    {
        return pathfinding.GetPath(map, map[NodeUtils.PositionToIndex(origin)], map[NodeUtils.PositionToIndex(destination)]);
    }
    #endregion
}