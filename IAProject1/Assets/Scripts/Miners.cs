using System.Collections.Concurrent;
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
    private float deltaTime;
    private readonly ConcurrentBag<Miner> miners = new();
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 12 };

        for (int i = 0; i < minersCount; i++)
        {
            GameObject go = Instantiate(minerGo, transform);
            Miner miner = go.GetComponent<Miner>();
            miner.Init(mine, deposit, rest, GetDeltaTime);

            miners.Add(miner);
        }
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            Parallel.ForEach(miners, parallelOptions, miner => { miner.ExitMiner(); });
        }

        Parallel.ForEach(miners, parallelOptions, miner => { miner.UpdateMiner(); });
    }
    #endregion

    #region PRIVATE_METHODS
    private float GetDeltaTime()
    {
        return deltaTime;
    }
    #endregion
}