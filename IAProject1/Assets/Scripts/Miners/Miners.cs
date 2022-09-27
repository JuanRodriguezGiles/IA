using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using UnityEngine;

public class Miners : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Vector3Int minerSpawnPos;
    [SerializeField] private GameObject minerGo;
    [SerializeField] private List<GameObject> mines;
    [SerializeField] private GameObject deposit;
    [SerializeField] private GameObject rest;
    #endregion

    #region PRIVATE_FIELDS
    private ConcurrentBag<Miner> miners = new();
    private ParallelOptions parallelOptions;
    private Vector2Int depositPos;
    private Vector2Int restPos;
    private float deltaTime;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 12 };

        depositPos = new Vector2Int((int)deposit.transform.position.x, (int)deposit.transform.position.y);
        restPos = new Vector2Int((int)rest.transform.position.x, (int)rest.transform.position.y);
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

        Parallel.ForEach(miners, parallelOptions, miner => { miner.UpdateMiner(); });
    }
    #endregion

    #region PUBLIC_METHODS
    public void SpawnMiner()
    {
        var go = Instantiate(minerGo, minerSpawnPos, Quaternion.identity, transform);
        var miner = go.GetComponent<Miner>();
        miner.Init(depositPos, restPos, miner.transform.position, GetDeltaTime, GetMine, OnEmptyMine);

        miners.Add(miner);
    }

    public void SpawnMine()
    {
    }
    #endregion

    #region PRIVATE_METHODS
    private float GetDeltaTime()
    {
        return deltaTime;
    }

    private Vector2Int GetMine()
    {
        int index = Random.Range(0, mines.Count);

        Vector2Int pos = new Vector2Int((int)mines[index].transform.position.x, (int)mines[index].transform.position.y);

        return pos;
    }

    private void OnEmptyMine(Vector2Int minePos)
    {
        Vector2Int pos;
        for (int i = 0; i < mines.Count; i++)
        {
            pos = new Vector2Int((int)mines[i].transform.position.x, (int)mines[i].transform.position.y);
            if (minePos == pos)
            {
                Destroy(mines[i]);
                mines.RemoveAt(i);
                break;
            }
        }
    }
    #endregion
}