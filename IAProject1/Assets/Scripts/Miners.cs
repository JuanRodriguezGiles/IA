using System.Collections.Concurrent;
using System.Threading.Tasks;

using UnityEngine;

public class Miners : MonoBehaviour
{
    #region EXPOSED_FIELDS
    public GameObject minerGo;
    public GameObject mine;
    public GameObject deposit;
    public GameObject rest;
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

        for (int i = 0; i < 100; i++)
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