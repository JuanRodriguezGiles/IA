using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using UnityEngine;

using Random = UnityEngine.Random;

public class Miners : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("Config")]
    [SerializeField] private Vector3Int minerSpawnPos;
    [SerializeField] private int minesCount;
    [SerializeField] private Transform minesParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject depositGo;

    [SerializeField] private GameObject minerGo;
    [SerializeField] private GameObject mineGo;
    [SerializeField] private GameObject restGo;
    #endregion

    #region PRIVATE_FIELDS
    [Header("Data")]
    [SerializeField] private List<Vector2Int> buildings = new();

    [SerializeField] private List<GameObject> mines;
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

        depositPos = new Vector2Int((int)depositGo.transform.position.x, (int)depositGo.transform.position.y);
        restPos = new Vector2Int((int)restGo.transform.position.x, (int)restGo.transform.position.y);

        buildings.Add(depositPos);

        for (int i = 0; i < mines.Count; i++)
        {
            Vector2Int pos = new Vector2Int((int)mines[i].transform.position.x, (int)mines[i].transform.position.y);
            buildings.Add(pos);
        }

        for (int i = 0; i < minesCount; i++)
        {
            SpawnMine();
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
    #endregion

    #region PRIVATE_METHODS
    private void SpawnMine()
    {
        int x = Random.Range(0, 50);
        int y = Random.Range(0, 50);
        Vector2Int pos = new Vector2Int(x, y);

        for (int i = 0; i < buildings.Count; i++)
        {
            if (pos == buildings[i])
            {
                SpawnMine();
                return;
            }
        }

        Vector3 posVec3 = new Vector3(pos.x, pos.y, 0);
        var go = Instantiate(mineGo, posVec3, Quaternion.identity, minesParent);
        
        mines.Add(go);
        buildings.Add(pos);
    }
    
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
                buildings.Remove(pos);
                Destroy(mines[i]);
                mines.RemoveAt(i);
                break;
            }
        }
    }
    #endregion
}