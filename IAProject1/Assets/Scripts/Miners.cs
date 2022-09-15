using System.Threading.Tasks;
using System.Collections.Concurrent;

using UnityEngine;

public class Miners : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private int minersCount = 1;
    [SerializeField] private Vector3Int minerSpawnPos;
    [SerializeField] private GameObject minerGo;
    [SerializeField] private GameObject mine;
    [SerializeField] private GameObject deposit;
    [SerializeField] private GameObject rest;
    #endregion

    #region PRIVATE_FIELDS
    private ParallelOptions parallelOptions;
    private ConcurrentBag<Miner> miners = new();

    private float deltaTime;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 12 };
        
        Vector2Int minePos = new Vector2Int((int)mine.transform.position.x, (int)mine.transform.position.y);        
        Vector2Int depositPos = new Vector2Int((int)deposit.transform.position.x, (int)deposit.transform.position.y);        
        Vector2Int restPos = new Vector2Int((int)rest.transform.position.x, (int)rest.transform.position.y);        
        
        for (var i = 0; i < minersCount; i++)
        {
            var go = Instantiate(minerGo, minerSpawnPos, Quaternion.identity, transform);
            var miner = go.GetComponent<Miner>();
            miner.Init(minePos, depositPos, restPos, miner.transform.position, GetDeltaTime);

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