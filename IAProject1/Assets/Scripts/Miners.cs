using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

public class Miners : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private int minersCount = 1;
    [SerializeField] private GameObject minerGo;
    [SerializeField] private GameObject mine;
    [SerializeField] private GameObject deposit;
    [SerializeField] private GameObject rest;
    #endregion

    #region PRIVATE_FIELDS
    private ParallelOptions parallelOptions;
    private ConcurrentBag<Miner> miners = new();

    private float deltaTime;

    private Pathfinding pathfinding;
    private Node[] map;
    #endregion

    #region UNITY_CALLS
    private void Start()
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
        
        pathfinding = new Pathfinding();
        //--------------------------------------------------------------------------------
        parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 12 };

        for (var i = 0; i < minersCount; i++)
        {
            var go = Instantiate(minerGo, transform);
            var miner = go.GetComponent<Miner>();
            miner.Init(mine, deposit, rest, miner.transform.position, GetDeltaTime);

            miners.Add(miner);
        }
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;

        foreach (var miner in miners)
        {
            if (miner.updatePos)
            {
                miner.transform.position = miner.currentPos;
                miner.updatePos = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Parallel.ForEach(miners, parallelOptions, miner => { miner.ExitMiner(); });
        }
        
        Parallel.ForEach(miners, parallelOptions, miner =>
        {
            miner.UpdateMiner();
        });
    }
    #endregion

    #region PRIVATE_METHODS
    private float GetDeltaTime()
    {
        return deltaTime;
    }
    #endregion
}