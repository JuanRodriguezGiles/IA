using System;
using System.Collections.Generic;

using UnityEngine;

public class GoToMine : FSMAction
{
    #region PRIVATE_FIELDS
    private readonly Vector3 mine;
    private readonly Transform miner;
    private const float speed = 10.0f;
    private readonly Func<float> onGetDeltaTime;
    private List<Vector2Int> path = null;
    private Pathfinding pathfinding;
    private Node[] map;
    private Vector3 currentDestination;
    private int posIndex = 0;
    #endregion

    #region OVERRIDE
    public override void Execute()
    {
        if (path == null)
        {
            map = new Node[10 * 10];
            NodeUtils.MapSize = new Vector2Int(10, 10);
            int ID = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map[ID] = new Node(ID, new Vector2Int(j, i));
                    ID++;
                }
            }
            
            map[NodeUtils.PositionToIndex(new Vector2Int(5, 3))].state = Node.NodeState.Obstacle;
            map[NodeUtils.PositionToIndex(new Vector2Int(4, 3))].state = Node.NodeState.Obstacle;
            map[NodeUtils.PositionToIndex(new Vector2Int(3, 3))].state = Node.NodeState.Obstacle;

            pathfinding = new Pathfinding();
            path = pathfinding.GetPath(map, map[NodeUtils.PositionToIndex(new Vector2Int((int)miner.position.x, (int)miner.position.y))],
                map[NodeUtils.PositionToIndex(new Vector2Int((int)mine.x, (int)mine.y))]);

            currentDestination = new Vector3(path[posIndex].x, path[posIndex].y, 0);
        }
        
        Vector2 dir = (currentDestination - miner.position).normalized;

        if (Vector2.Distance(currentDestination, miner.position) > 1.0f)
        {
            Vector2 movement = dir * (speed * onGetDeltaTime.Invoke());
            miner.position += new Vector3(movement.x, movement.y);
        }
        else if (posIndex >= path.Count - 1) 
        {
            onSetFlag?.Invoke((int)Flags.OnReachMine);
        }
        else
        {
            posIndex++;
            currentDestination = new Vector3(path[posIndex].x, path[posIndex].y, 0);
        }
    }
    #endregion

    #region CONSTRUCTOR
    public GoToMine(Action<int> onSetFlag, Func<float> onGetDeltaTime, Vector3 mine, Transform miner)
    {
        this.onGetDeltaTime = onGetDeltaTime;
        this.onSetFlag = onSetFlag;
        this.mine = mine;
        this.miner = miner;
    }
    #endregion
}